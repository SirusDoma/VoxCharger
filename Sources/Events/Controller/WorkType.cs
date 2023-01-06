using System;

namespace VoxCharger
{
    public abstract partial class Camera
    {
        public enum WorkType
        {
            None,
            Rotation,
            Radian,
            Realize,
            AirLeftScaleX,
            AirRightScaleX,
            AirLeftScaleY,
            AirRightScaleY,
            Tilt,
            LaneClear
        }
    }
}
