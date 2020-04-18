using System;

namespace VoxCharger
{
    public abstract partial class Camera
    {
        public class Radian : Camera
        {

            public Radian(Time time, int duration, float start, float end)
                : base(time, WorkType.Radian)
            {
                Duration = duration;
                Start    = start;
                End      = end;
            }

            public override string ToString()
            {
                return $"{base.ToString()}" +
                       $"\tCAM_Radi" +
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
