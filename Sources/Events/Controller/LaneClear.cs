using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxCharger
{
    public abstract partial class Camera
    {
        public class LaneClear : Camera
        {
            public LaneClear(Time time, int duration, float start, float end)
                : base(time, WorkType.LaneClear)
            {
                Duration = duration;
                Start = start;
                End   = end;
            }

            public override string ToString()
            {
                return $"{base.ToString()}" +
                       $"\tLaneY" +
                       $"\t2" +
                       $"\t{Duration:0.00}" +
                       $"\t{Start:0.00}" +
                       $"\t{End:0.00}" +
                       $"\t0.00" +
                       $"\t0.00";
            }
        }
    }
}
