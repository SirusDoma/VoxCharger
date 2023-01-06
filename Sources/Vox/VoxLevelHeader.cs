using System;
using System.Drawing;

namespace VoxCharger
{
    public class VoxLevelHeader
    {
        public Difficulty Difficulty { get; set; }
        public int Level             { get; set; } = 1;
        public string Illustrator    { get; set; } = "dummy";
        public string Effector       { get; set; } = "dummy";
        public int Price             { get; set; } = -1;
        public int Limited           { get; set; } = 3;

        public int JacketPrint       { get; set; } = -2;
        public int JacketMask        { get; set; } = 0;

        public VoxLevelRadar Radar   { get; set; }
        public VoxChart Chart        { get; set; }
        public Image Jacket          { get; set; }
    }

    public class VoxLevelRadar
    {
        public byte Notes    { get; set; }
        public byte Peak     { get; set; }
        public byte Lasers   { get; set; }
        public byte Tricky   { get; set; }
        public byte HandTrip { get; set; }
        public byte OneHand  { get; set; }
    }
}
