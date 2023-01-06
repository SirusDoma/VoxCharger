using System;

namespace VoxCharger
{
    public partial class VoxChart
    {
        public enum Section
        {
            NoState        = -1,
            FormatVersion  = 0,
            BeatInfo       = 1,
            BpmInfo        = 2,
            Tilt            = 3,
            Lyric           = 4,
            EndPosition    = 5,
            TabEffect      = 6,
            FxbuttonEffect = 7,
            TabParam       = 8,
            Reverb          = 9,
            Track1          = 10,
            Track2          = 11,
            Track3          = 12,
            Track4          = 13,
            Track5          = 14,
            Track6          = 15,
            Track7          = 16,
            Track8          = 17,
            TrackAuto      = 18,
            Spcontroler     = 19,
            SoundId        = 20,
            Bpm             = 21
        }

        public static bool IsTrackSection(Section section)
        {
            int value = (int)section;
            return value >= (int)Section.Track1 && value <= (int)Section.TrackAuto;
        }

        public static int GetTrackNumber(Section section)
        {
            return IsTrackSection(section) ? ((int)section + 1) - (int)Section.Track1 : -1;
        }
    }
}
