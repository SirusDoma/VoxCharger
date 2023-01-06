using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace VoxCharger
{
    public partial class Effect
    {
        public class Retrigger : Effect
        {
            public int WaveLength     { get; set; }
            public float Mix          { get; set; }
            public float UpdatePeriod { get; set; }
            public float Feedback     { get; set; }
            public float Rate         { get; set; }
            public float Tick         { get; set; }
            public bool  Updatable    { get; set; }

            public Retrigger(int waveLength, float mix, float updatePeriod, float feedback, float rate, float tick, bool updatable = false) 
                : base(updatable ? FxType.RetriggerEx : FxType.Retrigger)
            {
                WaveLength   = waveLength;
                Mix          = mix;
                UpdatePeriod = updatePeriod;
                Feedback     = feedback;
                Rate         = rate;
                Tick         = tick;
                Updatable    = updatable;
            }

            private Retrigger()
                : base(FxType.None)
            {
            }

            public new static Retrigger FromVox(string data)
            {
                var retrigger = new Retrigger();
                var prop = data.Trim().Split(',').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0], out FxType type) || (type != FxType.Retrigger && type != FxType.RetriggerEx))
                    return retrigger;

                if (prop.Length < 7)
                    return retrigger;

                try
                {
                    retrigger.WaveLength   = int.Parse(prop[1]);
                    retrigger.Mix          = float.Parse(prop[2]);
                    retrigger.UpdatePeriod = float.Parse(prop[3]);
                    retrigger.Feedback     = float.Parse(prop[4]);
                    retrigger.Rate         = float.Parse(prop[5]);
                    retrigger.Tick         = float.Parse(prop[6]);
                    retrigger.Updatable    = type == FxType.RetriggerEx;

                    retrigger.Type = type;
                }
                catch (Exception)
                {
                    retrigger.Type = FxType.None;
                }

                return retrigger;
            }

            public new static Retrigger FromKsh(string data)
            {
                var retrigger = new Retrigger();
                var prop = data.Trim().Split(';').Select(p => p.Trim()).ToArray();
                if (!Enum.TryParse(prop[0].Replace("Echo", "Retrigger"), out FxType type) || (type != FxType.Retrigger && type != FxType.RetriggerEx))
                    return retrigger;

                float waveLength = 8f;
                if (prop.Length > 1)
                    waveLength = float.TryParse(prop[1], out waveLength) ? waveLength : 8f;

                float defaultFeedback = prop[0] == "Echo" ? 0.6f : 1.0f;
                float feedback = defaultFeedback;
                if (prop.Length > 2)
                    feedback = float.TryParse(prop[2], out feedback) ? feedback : defaultFeedback;

                retrigger.WaveLength   = (int)(waveLength / 2);
                retrigger.Mix          = 100.0f;
                retrigger.UpdatePeriod = 2.00f;
                retrigger.Feedback     = 1.00f;
                retrigger.Rate         = 0.70f;
                retrigger.Tick         = 0.15f;
                retrigger.Updatable    = true;

                retrigger.Type         = FxType.RetriggerEx;

                return retrigger;
            }

            public new static Retrigger FromKsh(KshDefinition definition)
            {
                var retrigger = new Retrigger();

                try 
                { 
                    if (!definition.GetValue("mix", out float mix) || !definition.GetValue("updatePeriod", out float updatePeriod))
                        return retrigger;

                    retrigger.WaveLength   = definition.GetValue("waveLength", out int waveLength)    ? waveLength : 0;
                    retrigger.Mix          = mix;
                    retrigger.Feedback     = definition.GetValue("feedbackLevel", out float feedback) ? feedback   : 0f;
                    retrigger.Rate         = definition.GetValue("rate", out float rate)              ? rate       : 0f;
                    retrigger.UpdatePeriod = updatePeriod * 4f;
                    retrigger.Tick         = updatePeriod < 1.0f ? 1.0f - updatePeriod : 0f;
                    retrigger.Updatable    = definition.GetString("updateTrigger", out string _);
                    retrigger.Type         = retrigger.Updatable ? FxType.RetriggerEx : FxType.Retrigger;
                }
                catch (Exception)
                {
                    retrigger.Type = FxType.None;
                }

                return retrigger;
            }

            public override string ToString()
            {
                if (Type == FxType.None)
                    return base.ToString();

                return $"{(int)Type},"        +
                       $"\t{WaveLength},"       +
                       $"\t{Mix:0.00},"       +
                       $"\t{UpdatePeriod:0.00}," +
                       $"\t{Feedback:0.00},"  +
                       $"\t{Rate:0.00},"      +
                       $"\t{Tick:0.00}"       + 
                       (Updatable ? ",\t0.00" : string.Empty);
            }
        }
    }
}
