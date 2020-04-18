using System;

namespace VoxCharger
{
    public abstract partial class Camera
    {
        public class Rotation : Camera
        {
            public Rotation(Time time, int duration, float start, float end)
                : base(time, WorkType.Rotation)
            {
                Duration = duration;
                Start    = start;
                End      = end;
            }

            public override string ToString()
            {
                return $"{base.ToString()}" +
                       $"\tCAM_RotX" +
                       $"\t2" +
                       $"\t{Duration}" +
                       $"\t{Start:0.00}" +
                       $"\t{End:0.00}" +
                       $"\t0.00" +
                       $"\t0.00";
            }
        }
    }
}
