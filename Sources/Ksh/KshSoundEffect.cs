using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxCharger
{
    public partial class Ksh
    {
        public class KshSoundEffect
        {
            public Event.ButtonTrack Track { get; set; }
            public Event.ChipFx Effect     { get; set; } = Event.ChipFx.None;
            public int HitCount            { get; set; }
            public int Used                { get; set; }
        }
    }
}
