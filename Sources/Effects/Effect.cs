using System;
using System.Collections.Generic;

namespace VoxCharger
{
    public partial class Effect
    {
        public static readonly Effect Empty   = new Effect();

        public static readonly Effect Default = new Retrigger(24, 95f, 1f, 1f, 0.85f, 0.16f, true);

        public FxType Type { get; private set; }

        public int Id      { get; set; } = 1;

        public Effect()
            : this(FxType.None)
        {
        }

        public Effect(FxType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return "0,\t0,\t0,\t0,\t0,\t0,\t0";
        }

        public static Effect FromVox(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var prop = data.Trim().Split(',');
            if (!Enum.TryParse(prop[0], out FxType type))
                return null;

            switch(type)
            {
                case FxType.Retrigger:
                case FxType.RetriggerEx: return Retrigger.FromVox(data);
                case FxType.Gate:        return Gate.FromVox(data);
                case FxType.Phaser:      return Phaser.FromVox(data);
                case FxType.TapeStopEx:
                case FxType.TapeStop:    return TapeStop.FromVox(data);
                case FxType.SideChain:   return SideChain.FromVox(data);
                case FxType.Wobble:      return Wobble.FromVox(data);
                case FxType.BitCrusher:  return BitCrusher.FromVox(data);
                case FxType.PitchShift:  return PitchShift.FromVox(data);
                case FxType.LowPass:     return LowPass.FromVox(data);
                case FxType.Flanger:     return Flanger.FromVox(data);
                default:                 return new Effect();
            }
        }

        public static Effect FromKsh(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var prop = data.Trim().Split(',');
            if (!Enum.TryParse(prop[0].Replace("Echo", "Retrigger"), out FxType type))
                return null;

            switch(type)
            {
                case FxType.Retrigger:
                case FxType.RetriggerEx: return Retrigger.FromKsh(data);
                case FxType.Gate:        return Gate.FromKsh(data);
                case FxType.Phaser:      return Phaser.FromKsh(data);
                case FxType.TapeStopEx:
                case FxType.TapeStop:    return TapeStop.FromKsh(data);
                case FxType.SideChain:   return SideChain.FromKsh(data);
                case FxType.Wobble:      return Wobble.FromKsh(data);
                case FxType.PitchShift:  return PitchShift.FromKsh(data);
                case FxType.Flanger:     return Flanger.FromKsh(data);
                default:                 return Default;
            }
        }

        public static Effect FromKsh(KshDefinition definition)
        {
            if (!definition.GetString("type", out string type))
                return new Effect();

            switch(type)
            {
                case "Echo":
                case "Retrigger":  return Retrigger.FromKsh(definition);
                case "Gate":       return Gate.FromKsh(definition);
                case "Phaser":     return Phaser.FromKsh(definition);
                case "TapeStop":   return TapeStop.FromKsh(definition);
                case "SideChain":  return SideChain.FromKsh(definition);
                case "LowPass":
                case "Wobble":     return Wobble.FromKsh(definition);
                case "PitchShift":
                case "BitCrusher": return BitCrusher.FromKsh(definition);
                case "Flanger":    return Flanger.FromKsh(definition);
                default:           return new Effect();
            }
        }
    }
}
