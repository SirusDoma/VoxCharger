using System;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public enum LaserTrack
        {
            Left  = 1,
            Right = 8
        }

        public enum LaserFlag
        {
            Tick  = 0,
            Start = 1,
            End   = 2
        }

        public enum SlamImpact
        {
            None          = 0,
            Measure       = 1,
            HalfMeasure   = 2,
            ThreeBeat     = 3,
            TripleMeasure = 4,
            Swing         = 5
        }

        public enum LaserFilter
        {
            None       = -1,
            Peak       = 0,
            LowPass    = 1,
            HighPass   = 3,
            BitCrusher = 5
        }

        public enum SlamDirection
        {
            Left,
            Right
        }

        public class Laser : Event
        {
            public LaserTrack Track { get; set; }
            public int Offset { get; set; }
            public LaserFlag Flag { get; set; }
            public SlamImpact Impact { get; set; }
            public int Range { get; set; }
            public LaserFilter Filter { get; set; }
            public bool IsLaserSlam { get; set; }

            public Laser(Time time, LaserTrack track, int offset, LaserFlag flag, SlamImpact impact)
                : base (time)
            {
                Track  = track;
                Offset = offset;
                Flag   = flag;
                Impact = impact;
            }

            public Laser(Time time, LaserTrack track, int offset, LaserFlag flag, SlamImpact impact, LaserFilter filter, int range)
                : this(time, track, offset, flag, impact)
            {
                Filter = filter;
                Range  = range;
            }

            public override string ToString()
            {
                return $"{base.ToString()}\t{Offset}\t{(int)Flag}\t{(int)Impact}\t{(int)Filter}\t{Range}\t0";
            }
        }

        // TODO: Pairing like this tend to break stuffs, need more tests, or don't use it at all
        public class Slam : Event
        {
            public Laser Start { get; set; }
            public Laser End { get; set; }
            public LaserTrack Track => Start.Track;
            public SlamDirection Direction => Start.Offset > End.Offset ? SlamDirection.Left : SlamDirection.Right;

            public Slam(Time time, Laser start, Laser end)
                : base (time)
            {
                Start = start;
                End   = end;
            }
        }
    }
}
