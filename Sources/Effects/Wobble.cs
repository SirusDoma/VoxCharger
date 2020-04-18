using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class Wobble : Effect
        {
            public float Mix          { get; set; }
            public float LowFrequency { get; set; }
            public float HiFrequency  { get; set; }
            public float WaveLength   { get; set; }
            public float Resonance    { get; set; }
            public int   Flag         { get; set; } = 1;

            public Wobble(float mix, float lowFrequency, float hiFrequency, float waveLength, float resonance)
                : base(FxType.Wobble)
            {
                Mix          = mix;
                LowFrequency = lowFrequency;
                HiFrequency  = hiFrequency;
                WaveLength   = waveLength;
                Resonance    = resonance;
            }

            private Wobble()
                : base(FxType.None)
            {
            }

            public static new Wobble FromVox(string data)
            {
                var wobble = new Wobble();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.Wobble)
                    return wobble;

                if (prop.Length != 8)
                    return wobble;

                try
                {
                    wobble.Type         = type;
                    wobble.Mix          = float.Parse(prop[3]);
                    wobble.LowFrequency = float.Parse(prop[4]);
                    wobble.HiFrequency  = float.Parse(prop[5]);
                    wobble.WaveLength   = float.Parse(prop[6]);
                    wobble.Resonance    = float.Parse(prop[7]);
                }
                catch (Exception)
                {
                    wobble.Type = FxType.None;
                }

                return wobble;
            }

            public static new Wobble FromKsh(string data)
            {
                var wobble = new Wobble();
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.Wobble)
                    return wobble;

                wobble.Type         = type;
                wobble.Mix          = 100.00f;
                wobble.LowFrequency = 500.00f;
                wobble.HiFrequency  = 20000.00f;
                wobble.WaveLength   = 4.00f;
                wobble.Resonance    = 1.41f;

                return wobble;
            }

            public static new Wobble FromKsh(KshDefinition definition)
            {
                var wobble = new Wobble();

                try 
                {
                    definition.GetValue("mix",         out float mix);
                    definition.GetValue("loFreq",      out float lowFreq);
                    definition.GetValue("hiFreq",      out float highFreq);
                    definition.GetValue("waveLength",  out float waveLength);
                    definition.GetValue("resonance",   out float resonance);

                    wobble.Flag         = 3;
                    wobble.Mix          = mix;
                    wobble.LowFrequency = lowFreq;
                    wobble.HiFrequency  = highFreq;
                    wobble.WaveLength   = waveLength * 4;
                    wobble.Resonance    = resonance;
                    wobble.Type         = FxType.Wobble;
                }
                catch (Exception)
                {
                    wobble.Type = FxType.None;
                }

                return wobble;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},"           +
                       $"\t,0,\t{Flag},"         + // Unknown params 
                       $"\t{Mix:0.00},"          +
                       $"\t{LowFrequency:0.00}," +
                       $"\t{HiFrequency:0.00},"  +
                       $"\t{WaveLength:0.00},"   +
                       $"\t{Resonance:0.00}";
            }
        }
    }
}
