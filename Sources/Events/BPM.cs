using System;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public class BPM : Event
        {
            public float Value { get; set; }

            public bool IsStop { get; set; }

            public BPM(Time time, float value)
                : base(time)
            {
                Value = value;
            }

            public override string ToString()
            {
                return $"{base.ToString()}\t{Value:0.00}\t{4}" + (IsStop ? "-" : string.Empty);
            }
        }
    }
}
