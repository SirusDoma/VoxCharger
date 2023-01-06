using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class SideChain : Effect
        {
            public float Mix    { get; set; }
            public float Period { get; set; }
            public int Hold     { get; set; }
            public int Attack   { get; set; }
            public int Release  { get; set; }

            public SideChain(float mix, float period, int hold, int attack, int release)
                : base(FxType.SideChain)
            {
                Mix     = mix;
                Period  = period;
                Hold    = hold;
                Attack  = attack;
                Release = release;
            }

            public SideChain()
                : base(FxType.None)
            {
            }

            public new static SideChain FromVox(string data)
            {
                var sideChain = new SideChain();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.SideChain)
                    return sideChain;

                if (prop.Length != 6)
                    return sideChain;

                try
                {
                    sideChain.Type    = type;
                    sideChain.Mix     = float.Parse(prop[1]);
                    sideChain.Period  = int.Parse(prop[2]);
                    sideChain.Hold    = int.Parse(prop[3]);
                    sideChain.Attack  = int.Parse(prop[4]);
                    sideChain.Release = int.Parse(prop[5]);
                }
                catch (Exception)
                {
                    sideChain.Type = FxType.None;
                }

                return sideChain;
            }

            public new static SideChain FromKsh(string data)
            {
                var sideChain = new SideChain();
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || type != FxType.SideChain)
                    return sideChain;

                sideChain.Type    = type;
                sideChain.Mix     = 90.00f;
                sideChain.Period  = 1.00f;
                sideChain.Hold    = 45;
                sideChain.Attack  = 50;
                sideChain.Release = 60;

                return sideChain;
            }

            public new static SideChain FromKsh(KshDefinition definition)
            {
                var sideChain = new SideChain();

                try 
                {
                    definition.GetValue("mix",         out float mix);
                    definition.GetValue("period",      out float period);
                    definition.GetValue("holdTime",    out int hold);
                    definition.GetValue("attackTime",  out int attack);
                    definition.GetValue("releaseTime", out int release);

                    sideChain.Mix         = mix;
                    sideChain.Period      = period / 2f;
                    sideChain.Hold        = hold;
                    sideChain.Attack      = attack;
                    sideChain.Release     = release;
                    sideChain.Type        = FxType.SideChain;
                }
                catch (Exception)
                {
                    sideChain.Type = FxType.None;
                }

                return sideChain;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},"       +
                       $"\t{Mix:0.00},"      +
                       $"\t{Period:0.00}," +
                       $"\t{Hold},"          +
                       $"\t{Attack},"        +
                       $"\t{Release}";
            }
        }

    }
}
