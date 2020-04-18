using System;

namespace VoxCharger
{
    public abstract partial class Event
    {
        public class TimeSignature : Event
        {
            public int Beat { get; set; }
            public int Note { get; set; }

            public TimeSignature(Time time, int beat, int note)
                : base(time)
            {
                Beat = beat;
                Note = note;
            }

            public static implicit operator (int beat, int note)(TimeSignature signature)
            {
                return (signature.Beat, signature.Note);
            }

            public override string ToString()
            {
                return $"{base.ToString()}\t{Beat}\t{Note}";
            }
        }
    }
}
