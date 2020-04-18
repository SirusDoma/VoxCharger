using System;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public class Stop : Event
        {
            public int Duration { get; set; }

            public Stop(Time time)
                : base(time)
            {
            }

            public Stop(Time time, int duration)
                : base(time)
            {
                Duration = duration;
            }
        }
    }
}
