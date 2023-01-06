using System;

namespace VoxCharger
{
    public class Time
    {
        public static readonly Time Initial = new Time(1, 1, 0);

        public int Measure { get; set; }
        public int Beat    { get; set; }
        public int Offset  { get; set; }

        public Time()
            : this(1, 1, 0)
        {
        }

        public Time(int measure, int beat, int cell)
        {
            Measure = measure;
            Beat    = beat;
            Offset  = cell;
        }

        public static Time FromString(string input)
        {
            try
            {
                var data = input.Split(',');
                if (data.Length != 3)
                    return null;

                return new Time()
                {
                    Measure = int.Parse(data[0]),
                    Beat    = int.Parse(data[1]),
                    Offset  = int.Parse(data[2])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static Time FromOffset(float absOffset, (int beat, int note) signature)
        {
            float remaining = (absOffset % 192f) % (192f / signature.beat);
            float division  = ((192f / signature.beat) / signature.note);

            int measure = (int)(absOffset / 192f);
            int beat    = (int)((absOffset % 192f) / (192f / signature.beat)) + 1;
            int offset  = (int)Math.Round((remaining / division) * ((192f / 4f) / 4f));

            // Since the value rounded up, there's chance this happen (precision rounding problem)
            beat += offset / 48;
            offset %= 48;

            measure += beat / signature.beat;
            beat %= signature.beat;

            if (beat == 0)
            {
                measure -= 1;
                beat = signature.beat;
            }

            return new Time(measure, beat, offset);
        }

        public int GetAbsoluteOffset((int beat, int note) signature)
        {
            float offset = Measure * 192f;
            offset      += ((Beat - 1) * (192f / signature.beat));
            offset      += ((Offset / (192f / 4f)) * (192f / signature.beat));

            return (int)Math.Round(offset);
        }

        public int Difference(Time time, (int beat, int note) signature)
        {
            // Only effective when both using same time signature
            float tick = ((192f / 4f) * (4f / signature.note));
            return (int)((Measure - time.Measure) * (tick * signature.beat)  +
                         (Beat    - time.Beat)    *  tick  +
                         (Offset  - time.Offset));
        }

        public Time Add(int position, (int beat, int note) signature)
        {
            int measure = Measure + (int)(position / 192f);
            int beat    = Beat    + (int)((position % 192f) / (192f / signature.beat));

            float absolute = (position % 192f) % (192f / signature.beat);
            int offset     = Offset + (int)Math.Round((absolute / ((192f / signature.beat) / signature.note)) * ((192f / 4f) / 4f));

            beat   += offset / 48;
            offset %= 48;

            measure += beat / signature.beat;
            beat    %= signature.beat;

            if (beat == 0)
            {
                measure -= 1;
                beat = signature.beat;
            }

            return new Time(measure, beat, offset);
        }

        public Time Add(Time time, (int beat, int note) signature)
        {
            int measure = Measure + time.Measure;
            int beat    = Beat    + time.Beat;
            int offset  = Offset  + time.Offset;

            beat   += offset / 48;
            offset %= 48;

            measure += beat / signature.beat;
            beat    %= signature.beat;

            if (beat == 0)
            {
                measure -= 1;
                beat = signature.beat;
            }

            return new Time(measure, beat, offset);
        }

        public static bool operator ==(Time a, Time b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return ReferenceEquals(a, b);

            return a.Measure == b.Measure && a.Beat == b.Beat && a.Offset == b.Offset;
        }

        public static bool operator !=(Time a, Time b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return ReferenceEquals(a, b);

            return a.Measure != b.Measure || a.Beat != b.Beat || a.Offset != b.Offset;
        }

        public static Time operator +(Time time, int position)
        {
            return time.Add(position, (4, 4));
        }

        public static Time operator +(Time a, Time b)
        {
            return a.Add(b, (4, 4));
        }

        public override bool Equals(object obj)
        {
            return obj is Time time && time == this;
        }

        public override int GetHashCode()
        {
            return Measure + Beat + Offset;
        }

        public override string ToString()
        {
            return $"{Measure:D3},{Beat:D2},{Offset:D2}";
        }
    }
}
