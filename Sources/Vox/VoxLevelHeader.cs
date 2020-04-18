using System;
using System.Drawing;

namespace VoxCharger
{
    public class VoxLevelHeader
    {
        public Difficulty Difficulty { get; set; }
        public int Level { get; set; } = 1;
        public string Illustrator { get; set; } = "dummy";
        public string Effector { get; set; } = "dummy";
        public int Price { get; set; } = -1;
        public int Limited { get; set; } = 3;
        public VoxChart Chart { get; set; }
        public Image Jacket { get; set; }
    }
}
