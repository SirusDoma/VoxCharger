using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Utils;

namespace VoxCharger
{
    public static class DxEncoder
    {
        // We need a custom MS-ADPCM format to encode the wav to .2dx file later
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private class DxAdpcmWaveFormat : AdpcmWaveFormat
        {
            public DxAdpcmWaveFormat(int sampleRate, int channels)
                : base(sampleRate, channels)
            {
                // The parameters must be:
                // blockAlign = 256
                // samplePerBlock = 244
                // averageBytesPerSecond = (blockAlign * sampleRate / samplesPerBlock + 0.5)
                blockAlign = 256;

                // We could define the wave format from the scratch so we can assign custom samplesPerBlock
                // But Im too lazy for that, so I had to resort reflection to get this done
                var samplesPerBlockProp = typeof(AdpcmWaveFormat).GetField("samplesPerBlock", BindingFlags.NonPublic | BindingFlags.Instance);
                samplesPerBlockProp?.SetValue(this, (short)244);

                averageBytesPerSecond = (int)(blockAlign * sampleRate / (double)SamplesPerBlock + 0.5D);
            }
        }

        public static void Encode(string[] sources, string output, AudioImportOptions opt = null)
        {
            opt ??= AudioImportOptions.Default;

            var samples = sources.Select(source => ConvertWav(source, opt)).ToList();

            using var stream = File.Create(output);
            using var writer = new BinaryWriter(stream);

            string name = Path.GetFileName(output);
            name = name.Substring(0, Math.Min(name.Length, 16)).PadRight(16, '\0');
            int headerSize = 72 + sources.Length * sizeof(int);

            writer.Write(Encoding.UTF8.GetBytes(name));
            writer.Write(headerSize);
            writer.Write(sources.Length);
            writer.Write(new byte[48]);

            int offset = headerSize;
            for (int i = 0; i < sources.Length; i++)
            {
                writer.Write(offset);
                offset += (int)(samples[i].Length + 24);
            }

            for (int i = 0; i < sources.Length; i++)
            {
                using var sample = samples[i];
                writer.Write(Encoding.UTF8.GetBytes("2DX9"));
                writer.Write(24);
                writer.Write((int)sample.Length);
                writer.Write((short)12849);
                writer.Write((short)-1);
                writer.Write((short)64);
                writer.Write((short)0);
                writer.Write(0);
                writer.Write(sample.ToArray());
            }
        }

        private static MemoryStream ConvertWav(string fileName, AudioImportOptions opt)
        {
            // The conversion happen in 3 steps:
            // 1. Convert our source into a temporary PCM WAV, here I use 44.1khz 16bit @ 2 channels
            // 2. If the file is preview file, apply preview effects (trim and fading) to our temporary PCM, otherwise skip this step
            // 3. Convert our temporary PCM to MS-ADPCM (44.1khz 4bit @ 2channels)
            //
            // The `WaveFormatConversionStream` will likely to fail if we convert our source straight to MS-ADPCM
            // Also, NAudio doesn't support writing samples smaller than 16bit (our MS-ADPCM use 4 bit), so we had to put our preview effects while in 16bit format

            // Zero, define our target format and preview flag
            var format = new DxAdpcmWaveFormat(44100, 2); // it's 4 bits per sample
            bool preview = opt.IsPreview;
            int previewStart = opt.PreviewOffset; // in seconds format

            // First, convert OGG to PCM WAV 44.1k 16bit @ 2 channels
            // Note that, MediaFoundationReader is not supported in WinXP and subject to NAudio limitation, refer to their doc for more info
            WaveStream source;
            if (Path.GetExtension(fileName) == ".ogg")
                source = new VorbisWaveReader(fileName);
            else
                source = new MediaFoundationReader(fileName);

            // Initializes memory stream to store our temporary PCM 16bit
            // Use IgnoreDisposeStream, otherwise our memory stream will be disposed when disposing wav writer
            using var ms = new MemoryStream();
            var tmp = new IgnoreDisposeStream(ms); // No need `using`, disposing this thing won't dispose our MemoryStream

            // Initialize our PCM 44.1khz 16bit @ 2 channels
            using (var wav = new WaveFileWriter(tmp, new WaveFormat(44100, 16, 2)))
            {
                // We can use either IWaveProvider or ISampleProvider here doesn't matter
                // Just be aware the values you feed during the conversion (Read() use byte[] buffer vs float[] samples)
                IWaveProvider provider = source;
                if (preview)
                {
                    // Trim audio for preview purpose
                    provider = new OffsetSampleProvider(provider.ToSampleProvider())
                    {
                        SkipOver = TimeSpan.FromSeconds(previewStart),
                        Take = TimeSpan.FromSeconds(10)
                    }.ToWaveProvider();

                    // Add fade in effect in the beginning of audio for preview purpose
                    var fader = new FadeInOutSampleProvider(provider.ToSampleProvider());
                    fader.BeginFadeIn(1000);

                    provider = fader.ToWaveProvider();
                }

                // Convert our Source to 16bit Wave
                provider = new SampleToWaveProvider16(provider.ToSampleProvider());

                // You could probably use AverageBytesPerSecond * 2 here to store the whole second for 2 channels samples
                // Just don't set it too much, otherwise our fade-out checking won't hit 
                byte[] buffer = new byte[provider.WaveFormat.AverageBytesPerSecond];
                bool fadeOut = false;
                for (int read; (read = provider.Read(buffer, 0, buffer.Length)) > 0;)
                {
                    // Check whether it's preview or not, and if it is, we want to add fade out effect for the last second
                    if (preview && !fadeOut && wav.TotalTime.TotalMilliseconds >= 8000)
                    {
                        // Add fade out effect at the end of audio for preview purpose
                        var fader = new FadeInOutSampleProvider(provider.ToSampleProvider());
                        fader.BeginFadeOut(1000);
                        fadeOut = true;

                        // Sample it to 16 bit again to avoid distortion after adding fade out effect
                        provider = new SampleToWaveProvider16(fader);
                    }

                    // Write converted 16bit sample to the temporary memory stream
                    wav.Write(buffer, 0, read);
                }

            } // Ensure writer disposed so the data is completely flushed to our memory stream

            // Finally, convert PCM Wav to ADPCM Wav
            tmp.Seek(0, SeekOrigin.Begin); // reset offset so our WaveFileReader read our header correctly

            var output = new IgnoreDisposeStream(new MemoryStream());
            using (var pcm = new WaveFileReader(tmp.SourceStream))
            using (var adpcm = new WaveFormatConversionStream(format, pcm))
            using (var writer = new WaveFileWriter(output, format))
            {
                byte[] buffer = new byte[adpcm.Length];
                int read = adpcm.Read(buffer, 0, buffer.Length);
                writer.Write(buffer, 0, read);
            }

            return output.SourceStream as MemoryStream;
        }
    }
}
