using System;

namespace VoxCharger
{
    public abstract partial class Camera
    {
        public class Tilt : Camera
        {
            public Tilt(Time time, int duration, float start, float end)
                : base(time, WorkType.Tilt)
            {
                Duration = duration;
                Start    = start;
                End      = end;
            }

            public override string ToString()
            {
                return $"{base.ToString()}" +
                       $"\tTilt" +
                       $"\t2" +
                       $"\t{Duration}" +
                       $"\t{Start:0.00}" +
                       $"\t{End:0.00}" +
                       $"\t0.00" + // Can be 0.00, 2.00 or 3.00
                       $"\t0.00";
            }
        }
    }
}
