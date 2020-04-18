using System;
using System.Collections.Generic;

namespace VoxCharger
{
    public class VoxHeader
    {
        public int ID        { get; set; }
        public string Title  { get; set; }
        public string Artist { get; set; }
        public string Ascii  { get; set; }
        public double BpmMin { get; set; }
        public double BpmMax { get; set; }
        public DateTime DistributionDate { get; set; }
        public short Volume        { get; set; }
        public short BackgroundId  { get; set; }
        public int GenreId         { get; set; }
        public GameVersion Version { get; set; } = GameVersion.VividWave;
        public InfiniteVersion InfVersion { get; set; } = InfiniteVersion.MXM;
        public Dictionary<Difficulty, VoxLevelHeader> Levels { get; set; }

        public string CodeName => $"{ID:D4}_{Ascii}";

        public VoxHeader()
        {
        }

        public override string ToString()
        {
            return FixMappedChars(Title);
        }

        public static string FixMappedChars(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var map = new Dictionary<string, string>
            {
                {"\u203E", "~"},
                {"\u301C", "～"},
                {"\u49FA", "ê"},
                {"\u5F5C", "ū"},
                {"\u66E6", "à"},
                {"\u66E9", "è"},
                {"\u7F47", "ê"},
                {"\u8E94", "🐾"},
                {"\u9A2B", "á"},
                {"\u9A69", "Ø"},
                {"\u9A6B", "ā"},
                {"\u9A6A", "ō"},
                {"\u9AAD", "ü"},
                {"\u9B2F", "ī"},
                {"\u9EF7", "ē"},
                {"\u9F63", "Ú"},
                {"\u9F67", "Ä"},
                {"\u973B", "♠"},
                {"\u9F6A", "♣"},
                {"\u9448", "♦"},
                {"\u9F72", "♥"},
                {"\u9F76", "♡"},
                {"\u9F77", "é"},
                {"?壬", "êp"}
            };

            string result = input;
            foreach (var c in map)
                result = result.Replace(c.Key, c.Value);

            return result;
        }
    }
}
