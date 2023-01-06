using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class BitCrusher : Effect
        {
            public float Mix { get; set; }

            public int Reduction { get; set; }

            public BitCrusher(float mix, int reduction)
                : base(FxType.BitCrusher)
            {
                Mix       = mix;
                Reduction = reduction;
            }

            private BitCrusher()
                : base(FxType.None)
            {
            }

            public new static BitCrusher FromVox(string data)
            {
                var bitCrusher = new BitCrusher();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.BitCrusher)
                    return bitCrusher;

                if (prop.Length != 3)
                    return bitCrusher;

                try
                {
                    bitCrusher.Type      = type;
                    bitCrusher.Mix       = float.Parse(prop[1]);
                    bitCrusher.Reduction = int.Parse(prop[2]);
                }
                catch (Exception)
                {
                    bitCrusher.Type = FxType.None;
                }

                return bitCrusher;
            }

            public new static BitCrusher FromKsh(KshDefinition definition)
            {
                var bitCrusher = new BitCrusher();

                try 
                {
                    definition.GetValue("mix",       out float mix);
                    definition.GetValue("reduction", out int samples);

                    bitCrusher.Mix       = mix;
                    bitCrusher.Reduction = samples;
                    bitCrusher.Type      = FxType.BitCrusher;
                }
                catch (Exception)
                {
                    bitCrusher.Type = FxType.None;
                }

                return bitCrusher;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},\t{Mix:0.00},\t{Reduction}";
            }
        }
    }
}
