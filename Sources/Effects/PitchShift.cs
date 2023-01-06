using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class PitchShift : Effect
        {
            public float Mix       { get; set; }

            public float Reduction { get; set; }

            public PitchShift(float mix, float reduction)
                : base(FxType.PitchShift)
            {
                Mix       = mix;
                Reduction = reduction;
            }

            private PitchShift()
                : base(FxType.None)
            {
            }

            public new static PitchShift FromVox(string data)
            {
                var pitchShift = new PitchShift();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.PitchShift)
                    return pitchShift;

                if (prop.Length != 3)
                    return pitchShift;

                try
                {
                    pitchShift.Type      = type;
                    pitchShift.Mix       = float.Parse(prop[1]);
                    pitchShift.Reduction = float.Parse(prop[2]);
                }
                catch (Exception)
                {
                    pitchShift.Type = FxType.None;
                }

                return pitchShift;
            }

            public new static PitchShift FromKsh(string data)
            {
                var pitchShift = new PitchShift();
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.PitchShift)
                    return pitchShift;

                float pitch = 12f;
                if (prop.Length > 1)
                    pitch = float.TryParse(prop[1], out pitch) ? pitch : 12f;

                pitchShift.Type      = type;
                pitchShift.Mix       = 100.00f;
                pitchShift.Reduction = pitch;

                return pitchShift;
            }

            public new static PitchShift FromKsh(KshDefinition definition)
            {
                var pitchShift = new PitchShift();

                try 
                {
                    definition.GetValue("mix",       out float mix);
                    definition.GetValue("reduction", out float samples);

                    pitchShift.Mix       = mix;
                    pitchShift.Reduction = samples;
                    pitchShift.Type      = FxType.PitchShift;
                }
                catch (Exception)
                {
                    pitchShift.Type = FxType.None;
                }

                return pitchShift;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},\t{Mix:0.00},\t{Reduction:0.00}";
            }
        }
    }
}
