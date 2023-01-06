using System;
using System.Linq;

namespace VoxCharger
{
    public partial class Effect
    {
        public class TapeStop : Effect
        {
            public float Mix   { get; set; }
            public float Speed { get; set; }
            public float Rate  { get; set; }

            public TapeStop(float mix, float speed, float rate)
                : base(FxType.TapeStop)
            {
                Mix   = mix;
                Speed = speed;
                Rate  = rate;
            }

            private TapeStop()
                : base(FxType.None)
            {
            }

            public new static TapeStop FromVox(string data)
            {
                var tapeStop = new TapeStop();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || (type != FxType.TapeStop && type != FxType.TapeStopEx))
                    return tapeStop;

                if (prop.Length < 4)
                    return tapeStop;

                try
                {
                    tapeStop.Type  = type;
                    tapeStop.Mix   = float.Parse(prop[1]);
                    tapeStop.Speed = float.Parse(prop[2]);
                    tapeStop.Rate  = float.Parse(prop[3]);
                }
                catch (Exception)
                {
                    tapeStop.Type = FxType.None;
                }

                return tapeStop;
            }

            public new static TapeStop FromKsh(string data)
            {
                var tapeStop = new TapeStop();
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || (type != FxType.TapeStop && type != FxType.TapeStopEx))
                    return tapeStop;

                if (prop.Length < 4)
                    return tapeStop;

                float rate = 50f;
                if (prop.Length > 1)
                    rate = float.TryParse(prop[1], out rate) ? rate : 50f;

                rate = -(rate - 20 - 40) / 40f;
                if (rate < 0)
                    rate = 0.1f;

                tapeStop.Type  = type;
                tapeStop.Mix   = 100.00f;
                tapeStop.Speed = 8.00f;
                tapeStop.Rate  = rate;

                return tapeStop;
            }

            public new static TapeStop FromKsh(KshDefinition definition)
            {
                var tapeStop = new TapeStop();

                try 
                { 
                    if (!definition.GetValue("mix", out float mix) || !definition.GetValue("speed", out int speed))
                        return tapeStop;

                    tapeStop.Mix   = mix;
                    tapeStop.Speed = speed / 10f;
                    tapeStop.Rate  = (tapeStop.Speed / 2f) / 10f;
                    tapeStop.Type  = FxType.TapeStop;
                }
                catch (Exception)
                {
                    tapeStop.Type = FxType.None;
                }

                return tapeStop;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},\t{Mix:0.00},\t{Speed:0.00},\t{Rate:0.00}";
            }
        }

    }
}
