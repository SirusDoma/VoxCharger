using System;

namespace VoxCharger
{
    public partial class VoxChart
    {
        public enum Section
        {
            NO_STATE        = -1,
            FORMAT_VERSION  = 0,
            BEAT_INFO       = 1,
            BPM_INFO        = 2,
            TILT            = 3,
            LYRIC           = 4,
            END_POSITION    = 5,
            TAB_EFFECT      = 6,
            FXBUTTON_EFFECT = 7,
            TAB_PARAM       = 8,
            REVERB          = 9,
            TRACK1          = 10,
            TRACK2          = 11,
            TRACK3          = 12,
            TRACK4          = 13,
            TRACK5          = 14,
            TRACK6          = 15,
            TRACK7          = 16,
            TRACK8          = 17,
            TRACK_AUTO      = 18,
            SPCONTROLER     = 19,
            SOUND_ID        = 20,
            BPM             = 21
        }

        public static bool IsTrackSection(Section section)
        {
            int value = (int)section;
            return value >= (int)Section.TRACK1 && value <= (int)Section.TRACK_AUTO;
        }

        public static int GetTrackNumber(Section section)
        {
            return IsTrackSection(section) ? ((int)section + 1) - (int)Section.TRACK1 : -1;
        }
    }
}
