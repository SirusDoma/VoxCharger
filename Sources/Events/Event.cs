using System;
using System.Collections;
using System.Collections.Generic;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public Time Time { get; set; }

        protected Event(Time time)
        {
            Time = new Time(time.Measure, time.Beat, time.Offset);
        }

        public override string ToString()
        {
            return Time.ToString();
        }
    }
}
