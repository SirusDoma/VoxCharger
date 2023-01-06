using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class Gate : Effect
        {
            public float Mix  { get; set; }
            public int Length { get; set; }
            public float Rate { get; set; }

            public Gate(float mix, int length, float rate)
                : base(FxType.Gate)
            {
                Mix    = mix;
                Length = length;
                Rate   = rate;
            }

            private Gate()
                : base(FxType.None)
            {

            }

            public new static Gate FromVox(string data)
            {
                var gate = new Gate();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.Gate)
                    return gate;

                if (prop.Length != 4)
                    return gate;

                try
                {
                    gate.Type   = type;
                    gate.Mix    = float.Parse(prop[1]);
                    gate.Length = int.Parse(prop[2]);
                    gate.Rate   = float.Parse(prop[3]);
                }
                catch (Exception)
                {
                    gate.Type = FxType.None;
                }

                return gate;
            }

            public new static Gate FromKsh(string data)
            {
                var gate = new Gate();
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.Gate)
                    return gate;

                float waveLength = 8f;
                if (prop.Length > 1)
                    waveLength = float.TryParse(prop[1], out waveLength) ? waveLength : 8f;

                gate.Type   = type;
                gate.Mix    = 100.00f;
                gate.Length = (int)(waveLength / 2);
                gate.Rate   = 1.00f;

                return gate;
            }

            public new static Gate FromKsh(KshDefinition definition)
            {
                var gate = new Gate();

                try 
                { 
                    if (!definition.GetValue("mix", out float mix) || !definition.GetValue("waveLength", out int waveLength))
                        return gate;

                    gate.Mix    = mix;
                    gate.Length = waveLength / 2;
                    gate.Rate   = 1.00f;
                    gate.Type   = FxType.Gate;
                }
                catch (Exception)
                {
                    gate.Type = FxType.None;
                }

                return gate;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},\t{Mix:0.00},\t{Length},\t{Rate:0.00}";
            }
        }

    }
}
