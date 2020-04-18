using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxCharger
{
    public abstract partial class Camera : Event
    {
        public WorkType Work { get; private set; } = WorkType.None;
        public int Duration { get; set; }
        public float Start { get; set; }
        public float End { get; set; }

        public Camera(Time time, WorkType work)
            : base(time)
        {
            Work = work;
        }

        public static Camera Create(WorkType work, Time time, int duration, float start, float end)
        {
            switch (work)
            {
                case WorkType.Rotation:  return new Rotation(time, duration, start, end);
                case WorkType.Radian:    return new Radian(time, duration, start, end);
                case WorkType.Tilt:      return new Tilt(time, duration, start, end);
                case WorkType.LaneClear: return new LaneClear(time, duration, start, end);
            }

            return null;
        }
    }
}
