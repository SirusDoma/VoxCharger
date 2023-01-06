using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class LowPass : Effect
        {
            public float Mix { get; set; }
            public float Frequency { get; set; }

            public LowPass(float mix, float frequency)
                : base(FxType.LowPass)
            {
                Mix       = mix;
                Frequency = frequency;
            }

            private LowPass()
                : base(FxType.None)
            {
            }

            public new static LowPass FromVox(string data)
            {
                var lowPass = new LowPass();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.LowPass)
                    return lowPass;

                if (prop.Length != 5)
                    return lowPass;

                try
                {
                    lowPass.Type      = type;
                    lowPass.Mix       = float.Parse(prop[1]);
                    lowPass.Frequency = float.Parse(prop[3]);
                }
                catch (Exception)
                {
                    lowPass.Type = FxType.None;
                }

                return lowPass;
            }

            
            public new static LowPass FromKsh(KshDefinition definition)
            {
                var lowPass = new LowPass();

                try 
                {
                    definition.GetValue("loFreq", out int loFreq);
                    definition.GetValue("hiFreq", out int hiFreq);

                    lowPass.Mix       = 100f;
                    lowPass.Frequency = loFreq != 0f ? loFreq : hiFreq;
                    lowPass.Type      = FxType.LowPass;
                }
                catch (Exception)
                {
                    lowPass.Type = FxType.None;
                }

                return lowPass;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},"        +
                       $"\t{Mix:0.00},"       +
                       $"\t0,"                + // Unknown param
                       $"\t{Frequency:0.00}," +
                       $"\t0";                  // Unknown too
            }
        }
    }
}
