using System;
using System.Diagnostics;
using System.IO;

namespace VoxCharger
{
    public static class S3VTool
    {
        public static string ConverterFileName { get; set; } = "ffmpeg.exe";

        public static void Convert(string inputFileName, string outputFileName, AudioImportOptions opt = null)
        {
            opt = opt ?? AudioImportOptions.Default;
            if (!File.Exists(ConverterFileName))
                throw new FileNotFoundException($"{ConverterFileName} not found", ConverterFileName);

            /*
             * TODO: DO NOT USE! Original .s3v (.asf) file seems encoded by Windows Media Audio Pro 10 encoder with close to Lossless quality.
             *       FFMPEG doesn't support this, and thus, this will generate non-working audio file for the game
             *
             * Format: 256000 Bit Rate with 44100 Sample Rate 24 Bit depth @ 2 Channels
             */
            string previewArgs = "";
            if (opt.IsPreview)
                previewArgs = $"-ss {opt.PreviewOffset / 60:00}:{opt.PreviewOffset % 60:00} -t 10 -af afade=t=in:st={opt.PreviewOffset}:d=1,afade=t=out:st={opt.PreviewOffset + 9}:d=1";
            string args = $"-y -i \"{inputFileName}\" {previewArgs} -maxrate 297k -minrate 297k -bufsize 297 -vb 280k -ab 380k -ac 2 -ar 44100 -f asf \"{outputFileName}\"";

            var info = new ProcessStartInfo()
            {
                FileName               = ConverterFileName,
                Arguments              = args,
                WorkingDirectory       = Environment.CurrentDirectory,
                CreateNoWindow         = true,
                UseShellExecute        = false,
                RedirectStandardOutput = true,
                RedirectStandardError  = true
            };

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    int exitCode  = process.ExitCode;
                    string stdout = (process.StandardOutput.ReadToEnd() + ' ' +  process.StandardError.ReadToEnd().Trim()).Trim();
                    if (string.IsNullOrEmpty(stdout))
                        stdout = "Unknown error.";

                    throw new ApplicationException($"{Path.GetFileName(ConverterFileName)} execution failed.\n({exitCode}): {stdout}");
                }
            }
        }
    }
}