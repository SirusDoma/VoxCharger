using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class Flanger : Effect
        {
            public float Mix     { get; set; }
            public float Samples { get; set; }
            public float Depth   { get; set; }
            public float Period  { get; set; }

            public Flanger(float mix, float samples, float depth, float period)
                : base(FxType.Flanger)
            {
                Mix     = mix;
                Samples = samples;
                Depth   = depth;
                Period  = period;
            }

            public Flanger()
                : base(FxType.None)
            {
            }

            public static new Flanger FromVox(string data)
            {
                var highPass = new Flanger();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.Flanger)
                    return highPass;

                if (prop.Length != 5)
                    return highPass;

                try
                {
                    highPass.Type    = type;
                    highPass.Mix     = float.Parse(prop[1]);
                    highPass.Samples = float.Parse(prop[2]);
                    highPass.Depth   = float.Parse(prop[3]);
                    highPass.Period  = float.Parse(prop[4]);
                }
                catch (Exception)
                {
                    highPass.Type = FxType.None;
                }

                return highPass;
            }

            public static new Phaser FromKsh(string data)
            {
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.Flanger)
                    return null;

                return new Phaser(80.00f, 2.00f, 0.50f, 90, 2.00f);
            }

            public static new Flanger FromKsh(KshDefinition definition)
            {
                var flanger = new Flanger();

                try 
                {
                    definition.GetValue("mix",    out float mix);
                    definition.GetValue("depth",  out int depth);
                    definition.GetValue("period", out float period);

                    flanger.Mix     = mix;
                    flanger.Samples = int.TryParse(definition.Value, out int samples) ? samples * 10 : 0;
                    flanger.Depth   = depth;
                    flanger.Period  = period;
                    flanger.Type    = FxType.Flanger;
                }
                catch (Exception)
                {
                    flanger.Type = FxType.None;
                }

                return flanger;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},"      +
                       $"\t{Mix:0.00},"     +
                       $"\t{Samples:0.00}," +
                       $"\t{Depth:0.00},"   +
                       $"\t{Period:0.00}";
            }
        }

    }
}
