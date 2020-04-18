using System;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public enum ButtonTrack
        {
            A   = 3,
            B   = 4,
            C   = 5,
            D   = 6,
            FxL = 2,
            FxR = 7
        }

        public enum ChipFx 
        {
            None       = 0,
            Fx2        = 2,
            Clap       = 4,
            ClapImpact = 3,
            ClapPunchy = 5,
            Snare      = 6,
            SnareLow   = 8,
            Fx7        = 7,
            Fx9        = 9,
            Fx10       = 10,
            Fx11       = 11,
            Fx12       = 12,
            Fx13       = 13,
            Fx14       = 14
        }

        public class Button : Event
        {
            public ButtonTrack Track { get; set; }
            public int HoldLength    { get; set; }
            public Effect HoldFx     { get; set; }
            public ChipFx HitFx      { get; set; }
            public bool IsFx => Track == ButtonTrack.FxL || Track == ButtonTrack.FxR;

            public Button(Time time, ButtonTrack track, int holdLength, Effect holdFx = null, ChipFx chipFx = ChipFx.None)
                : base (time)
            {
                Track      = track;
                HoldLength = holdLength;
                HoldFx     = holdFx;
                HitFx      = chipFx;
            }

            public override string ToString()
            {
                int fx = 0;
                if (HoldFx != null)
                    fx = HoldFx.Id;
                else
                    fx = (int)HitFx;

                return $"{base.ToString()}\t{HoldLength}\t{fx}";
            }
        }
    }
}
