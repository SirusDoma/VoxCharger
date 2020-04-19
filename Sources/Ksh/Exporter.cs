using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxCharger
{
    

    public class ChartInfo
    {
        public Ksh Source { get; set; }
        public VoxLevelHeader Header { get; set; }
        public string FileName { get; set; }
        public string MusicFileName { get; set; }

        public ChartInfo(Ksh source, VoxLevelHeader header, string fileName)
        {
            Source = source;
            Header = header;
            FileName = fileName;
        }
    }

    public partial class Ksh
    {
        public class Exporter
        {
            public Ksh Source       { get; private set; } = null;
            public Action Action    { get; private set; } = null;

            public Exporter(Ksh src)
            {
                Source = src;
            }

            public void Export(VoxHeader header, ParseOption options = null)
            {
                Export(header, null, options);
            }

            public void Export(VoxHeader header, Dictionary<Difficulty, ChartInfo> charts, ParseOption options = null)
            {
                // Assign level info and charts
                foreach (var chart in charts)
                {
                    var info = chart.Value;
                    if (!File.Exists(info.FileName))
                        throw new IOException($"Chart file was moved or deleted\n{info.FileName}");

                    // If you happen to read the source, this is probably what you're looking for
                    var ksh = new Ksh();
                    ksh.Parse(info.FileName, options);

                    // Conversion is actually boring because its already "pre-converted"
                    var vox = new VoxChart();
                    vox.Import(ksh);

                    var level   = ksh.ToLevelHeader();
                    level.Chart = vox;

                    info.Source = ksh;
                    header.Levels[chart.Key] = charts[chart.Key].Header = level;
                }

                // Prevent resource files being moved or deleted, copy them into temporary storage
                string musicFile = string.Empty;
                string tmpFile = string.Empty;
                foreach (var chart in charts)
                {
                    var info = chart.Value;

                    // Make sure to reuse 2dx file for music that share same file
                    if (string.IsNullOrEmpty(musicFile) || chart.Value.MusicFileName != musicFile)
                    {
                        string music = Path.Combine(Path.GetDirectoryName(info.FileName), info.Source.MusicFileName);
                        if (File.Exists(music))
                        {
                            string tmp = Path.Combine(
                                Path.GetTempPath(),
                                $"{Path.GetRandomFileName()}{new FileInfo(info.Source.MusicFileName).Extension}"
                            );

                            musicFile = music;
                            info.MusicFileName = tmpFile = tmp;

                            File.Copy(music, tmp);
                        }
                        else
                            info.MusicFileName = string.Empty;
                    }
                    else
                        info.MusicFileName = tmpFile;

                    string jacket = Path.Combine(Path.GetDirectoryName(info.FileName), info.Source.JacketFileName);
                    if (File.Exists(jacket))
                    {                       
                        try
                        {
                            using (var image = Image.FromFile(jacket))
                                info.Header.Jacket = new Bitmap(image);
                        }
                        catch (Exception)
                        {
                            info.Header.Jacket = null;
                        }                    
                    }
                }

                Action = new Action(() =>
                {
                    bool unique = false;
                    musicFile = charts.Values.First().MusicFileName;
                    foreach (var chart in charts)
                    {
                        if (chart.Value.MusicFileName != musicFile)
                        {
                            unique = true;
                            break;
                        }
                    }

                    // Import all music assets
                    AssetManager.ImportVox(header);

                    // Make sure to use single asset for music for shared music file
                    if (!unique)
                    {
                        AssetManager.Import2DX(musicFile, header);
                        AssetManager.Import2DX(musicFile, header, true);
                    }

                    foreach (var chart in charts.Values)
                    {
                        if (unique && File.Exists(chart.MusicFileName))
                        {
                            AssetManager.Import2DX(chart.MusicFileName, header, chart.Header.Difficulty);
                            AssetManager.Import2DX(chart.MusicFileName, header, chart.Header.Difficulty, true);
                        }

                        if (chart.Header.Jacket != null)
                            AssetManager.ImportJacket(header, chart.Header.Difficulty, chart.Header.Jacket);
                    }
                });
            }

            public static Dictionary<Difficulty, ChartInfo> GetCharts(string dir, string title)
            {
                // Try to locate another difficulty
                var charts = new Dictionary<Difficulty, ChartInfo>();
                foreach (string fn in Directory.GetFiles(dir, "*.ksh"))
                {
                    try
                    {
                        var chart = new Ksh();
                        chart.Parse(fn);

                        // Different chart
                        if (chart.Title != title)
                            continue;

                        charts[chart.Difficulty] = new ChartInfo(chart, chart.ToLevelHeader(), fn);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed attempt to parse ksh file: {0} ({1})", fn, ex.Message);
                    }
                }

                return charts;
            }

        }
    }
}
