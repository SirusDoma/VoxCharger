using System;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public enum TiltType
        {
            Normal      = 0,
            Large       = 1,
            Incremental = 2
        }

        public class TiltMode : Event
        {
            public TiltType Mode { get; set; }

            public TiltMode(Time time, TiltType mode)
                : base(time)
            {
                Mode = mode;
            }

            public override string ToString()
            {
                return $"{base.ToString()}\t{(int)Mode}";
            }
        }
    }
}
