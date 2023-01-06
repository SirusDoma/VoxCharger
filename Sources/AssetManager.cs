using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace VoxCharger
{
    public enum AudioFormat
    {
        Iidx,
        S3V
    }

    public class AudioImportOptions
    {
        public static readonly AudioImportOptions Default = new AudioImportOptions();

        public AudioFormat Format { get; set; } = AudioFormat.Iidx;
        public bool IsPreview     { get; set; } = false;
        public int PreviewOffset  { get; set; } = 0;

        public static AudioImportOptions WithFormat(AudioFormat format)
        {
            return new AudioImportOptions
            {
                Format        = format,
                IsPreview     = false,
                PreviewOffset = 0
            };
        }

        public AudioImportOptions AsPreview()
        {
            return new AudioImportOptions
            {
                Format        = Format,
                IsPreview     = true,
                PreviewOffset = PreviewOffset
            };
        }
    }

    public static class AssetManager
    {
        #region --- Properties ---
        private static List<string> _mixList = new List<string>();
        private static MusicDb _internalHeaders = null;
        private static int _lastOriginalId = 0;

        public static string MixName { get; private set; }

        public static string GamePath { get; private set; }

        public static string MixPath { get; set; }

        public static MusicDb Headers { get; private set; } = new MusicDb();

        public static string MdbFilename { get; private set; }
        #endregion

        #region --- Mix Management ---
        public static void Initialize(string gamePath)
        {
            // Validate the existence of main music db
            string dbFilename = Path.Combine(gamePath, @"data\others\music_db.xml");
            if (!File.Exists(dbFilename))
                throw new FormatException("Invalid Game Directory");

            // Look for other mixes
            _mixList.Clear();
            string modsPath = Path.Combine(gamePath, @"data_mods\");
            foreach (var modDir in Directory.GetDirectories(modsPath))
            {
                // Get directory name
                string modName = new DirectoryInfo(modDir).Name;
                if (modName == "_cache")
                    continue;

                // Validate whether the mod is a mix mod
                dbFilename = Path.Combine(modDir, @"others\music_db.merged.xml");
                if (!File.Exists(dbFilename))
                    continue;

                // Only supports latest KFC datecode with music folder
                if (!Directory.Exists(Path.Combine(modDir, @"music\")))
                    continue;

                // Confirmed mod path, append into music db, ignore cache to avoid uncached mix being excluded
                _mixList.Add(modName);
            }

            _lastOriginalId = 0;
            GamePath = gamePath;
        }

        public static void CreateMix(string mixName)
        {
            string mixPath = Path.Combine(GamePath, @"data_mods\", mixName);
            if (Directory.Exists(mixPath))
                throw new IOException("Mix directory is already exists");

            // Create necessary directory
            Directory.CreateDirectory(Path.Combine(mixPath, @"graphics\s_jacket00_ifs\"));
            Directory.CreateDirectory(Path.Combine(mixPath, @"music\"));
            Directory.CreateDirectory(Path.Combine(mixPath, @"others\"));

            // Load Existing song DB to avoid duplicate id
            LoadInternalDb(mixName);

            // Create empty db
            MdbFilename = Path.Combine(mixPath, @"others\music_db.merged.xml");
            File.WriteAllText(MdbFilename, "<?xml version=\"1.0\" encoding=\"Shift_JIS\"?><mdb></mdb>");
            Headers.Clear();
            
            MixName = mixName;
            MixPath = mixPath;
        }

        public static void LoadMix(string mixName)
        {
            string dataPath = string.IsNullOrEmpty(mixName) ? @"data\" : @"data_mods\";
            string mixPath  = Path.Combine(GamePath, dataPath, mixName);
            if (!Directory.Exists(mixPath))
                throw new DirectoryNotFoundException("Mix directory missing");

            // Load Existing song DB to avoid duplicate id
            LoadInternalDb(mixName);
            if (!string.IsNullOrEmpty(mixName) && !_mixList.Contains(mixName))
                _mixList.Add(mixName);

            // Locate the music db, if unavailable, create it
            MdbFilename = Path.Combine(mixPath, @"others\", string.IsNullOrEmpty(mixName) ? "music_db.xml" : "music_db.merged.xml");
            Headers.Load(MdbFilename);
            
            MixName = mixName;
            MixPath = mixPath;
        }

        public static string[] GetMixes()
        {
            return _mixList.ToArray();
        }

        private static void LoadInternalDb(string mixName)
        {
            // First, we need to populate available ids outside selected mix
            // This will prevent duplicate ID not only with originals but with other mixes too

            // Load original music,  ignore cache to avoid uncached mix being excluded or included
            string dbFilename = Path.Combine(GamePath, @"data\others\music_db.xml");
            string modsPath = Path.Combine(GamePath, @"data_mods\");

            // Load original headers data
            _internalHeaders = new MusicDb();
            _internalHeaders.Load(dbFilename);
            _lastOriginalId  = _internalHeaders.LastId;

            // Load other music db
            foreach (var modDir in Directory.GetDirectories(modsPath))
            {
                // Get directory name, exclude selected mix
                string modName = new DirectoryInfo(modDir).Name;
                if (modName == "_cache" || modName == mixName)
                    continue;

                // Validate whether the mod is a mix mod
                // Do not skip unsupported mods, just read the db and ignore the rest of assets
                dbFilename = Path.Combine(modDir, @"others\music_db.merged.xml");
                if (!File.Exists(dbFilename))
                    continue;

                // Confirmed mod path, append into music db
                _internalHeaders.Load(dbFilename, true);
            }
        }
        #endregion

        #region --- Asset Management ---

        public static void ImportAudio(string source, VoxHeader header, AudioImportOptions opt = null)
        {
            opt = opt ?? AudioImportOptions.Default;
            if (!File.Exists(source))
                throw new FileNotFoundException($"{source} not found", source);

            ImportAudio(source, GetAudioPath(header, opt.Format, opt.IsPreview), opt);
        }

        public static void ImportAudio(string source, VoxHeader header, Difficulty difficulty, AudioImportOptions opt = null)
        {
            opt = opt ?? AudioImportOptions.Default;
            if (!File.Exists(source))
                throw new FileNotFoundException($"{source} not found", source);

            ImportAudio(source, GetAudioPath(header, difficulty, opt.Format, opt.IsPreview), opt);
        }

        private static void ImportAudio(string source, string output, AudioImportOptions opt = null)
        {
            opt = opt ?? AudioImportOptions.Default;
            source = source.ToLower(); // Just in case something wrong with extension casing
            if (!source.ToLower().EndsWith(".2dx") && !source.EndsWith(".s3v") && !source.EndsWith(".asf"))
            {
                if (output.EndsWith(".s3v"))
                    throw new NotSupportedException("S3V Encoder is not supported (yet).");

                // Encode 2dx file
                DxEncoder.Encode(new[] { source }, output, opt);

                // Remove conflicting file, in case s3v file is exists before hand
                string s3VFile = Path.ChangeExtension(output, ".s3v");
                if (File.Exists(s3VFile))
                    File.Delete(s3VFile);
            }
            else
            {
                if (source.EndsWith(".asf"))
                    output = $"{output.Substring(0, output.Length - 4)}.s3v";
                
                File.Copy(source, output);
            }
        }

        public static void ImportJacket(VoxHeader header, Image image)
        {
            ImportJacket(header, Difficulty.Novice, image);
        }

        public static void ImportJacket(VoxHeader header, Difficulty diff, Image image)
        {
            // Texture Small
            Image texSmall = new Bitmap(image, new Size(130, 130));
            texSmall.Save($"{GetThumbnailJacketPath(header, diff)}.png", ImageFormat.Png);
            texSmall.Dispose();
            texSmall = new Bitmap(image, new Size(108, 108));
            texSmall.Save($"{GetJacketPath(header, diff)}_s.png", ImageFormat.Png);
            texSmall.Dispose();

            // Texture Big
            Image texBig = new Bitmap(image, new Size(676, 676));
            texBig.Save($"{GetJacketPath(header, diff)}_b.png", ImageFormat.Png);
            texBig.Dispose();

            // Texture Normal
            Image texNormal = new Bitmap(image, new Size(300, 300));
            texNormal.Save($"{GetJacketPath(header, diff)}.png", ImageFormat.Png);
            texNormal.Dispose();
        }


        public static void ImportVox(VoxHeader header)
        {
            Directory.CreateDirectory(GetMusicPath(header));
            foreach (var level in header.Levels.Values)
            {
                if (level.Chart == null)
                    continue;

                var path = GetVoxPath(header, level.Difficulty);
                level.Chart.Serialize(path);
            }
        }

        public static void ImportVox(VoxHeader header, Difficulty diff, VoxChart chart)
        {
            Directory.CreateDirectory(GetMusicPath(header));
            chart.Serialize(GetVoxPath(header, diff));
        }

        public static void ImportVox(VoxHeader header, Difficulty diff, string content)
        {
            File.WriteAllText(GetVoxPath(header, diff), content);
        }

        public static void DeleteAssets(VoxHeader header)
        {
            string musicPath = GetMusicPath(header);
            if (Directory.Exists(musicPath))
                Directory.Delete(musicPath, true);

            string jacketPath = Path.Combine(
                MixPath,
                $"graphics\\s_jacket00_ifs\\"
            );

            string pattern = $"jk_{header.Id:D4}*";
            foreach (string jacket in Directory.GetFiles(jacketPath, pattern))
            {
                if (File.Exists(jacket))
                    File.Delete(jacket);
            }
        }
        #endregion

        #region --- Asset Identifier ---
        public static int GetNextMusicId()
        {
            // TODO: This probably inefficient in scenario where one or more mix has gap between each ids
            // This could be waste to those gaps, and eat up our precious limited id
            // However, these gaps may indicate deleted song, which should be taken by omnimix

            int id = _internalHeaders.LastId + 1;
            while (_internalHeaders.Contains(id) || Headers.Contains(id)) // Contains is O(1) so its should be fine
                id++;

            return id;
        }

        public static bool ValidateMusicId(int id)
        {
            return !_internalHeaders.Contains(id);
        }

        public static string GetDifficultyCodes(VoxHeader header, Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Novice:   return "1n";
                case Difficulty.Advanced: return "2a";
                case Difficulty.Exhaust:  return "3e";
                default:
                    if (header.InfVersion == InfiniteVersion.Mxm)
                        return "5m";

                    return "4i";
            }
        }

        public static string GetMusicPath(VoxHeader header)
        {
            return Path.Combine(
                MixPath,
                $"music\\{header.CodeName}\\"
            );
        }

        public static string GetVoxPath(VoxHeader header, Difficulty difficulty)
        {
            return Path.Combine(
                GetMusicPath(header), 
                $"{header.CodeName}_{GetDifficultyCodes(header, difficulty)}.vox"
            );
        }

        public static string GetAudioPath(VoxHeader header, AudioFormat format = AudioFormat.S3V, bool preview = false)
        {
            string prefix = preview ? "_pre" : string.Empty;
            string ext    = $"{prefix}" + (format == AudioFormat.S3V ? ".s3v" : ".2dx");
            return Path.Combine(
                GetMusicPath(header),
                $"{header.CodeName}{ext}"
            );
        }

        public static string GetAudioPath(VoxHeader header, Difficulty difficulty, AudioFormat format = AudioFormat.S3V, bool preview = false)
        {
            string prefix = preview ? "_pre" : string.Empty;
            string ext    = $"{prefix}" + (format == AudioFormat.S3V ? ".s3v" : ".2dx");
            return Path.Combine(
                GetMusicPath(header),
                $"{header.CodeName}_{GetDifficultyCodes(header, difficulty)}{ext}"
            );
        }

        public static string GetJacketPath(VoxHeader header, Difficulty difficulty)
        {
            int index = (int)difficulty;
            if (difficulty == Difficulty.Infinite && header.InfVersion == InfiniteVersion.Mxm)
                index = 5;

            return Path.Combine(GetMusicPath(header), $"jk_{header.Id:D4}_{index}");
        }

        public static string GetDefaultJacketPath(VoxHeader header)
        {
            return GetJacketPath(header, Difficulty.Novice);
        }

        public static string GetThumbnailJacketPath(VoxHeader header, Difficulty difficulty)
        {
            int index = (int)difficulty;
            if (difficulty == Difficulty.Infinite && header.InfVersion == InfiniteVersion.Mxm)
                index = 5;

            string thumbnailDir = "s_jacket00_ifs";
            string graphicsDir  = Path.Combine(MixPath, @"graphics\");

            if (Directory.Exists(graphicsDir))
            {
                foreach (string dir in Directory.GetDirectories(graphicsDir))
                {
                    string name = new DirectoryInfo(dir).Name;
                    if (name.StartsWith("s_jacket") && name.EndsWith("_ifs"))
                    {
                        thumbnailDir = name;
                        break;
                    }
                }
            }

            return Path.Combine(
                graphicsDir,
                $"{thumbnailDir}\\",
                $"jk_{header.Id:D4}_{index}_t"
            );
        }

        public static string GetDefaultThumbnailJacketPath(VoxHeader header)
        {
            return GetThumbnailJacketPath(header, Difficulty.Novice);
        }

        #endregion

        #region --- Utilities ---

        
        #endregion
    }
}
