using System;

namespace Graviris;
public enum RotateAngle
{
    ZERO = 0,
    RIGHT90 = 90,
    REVERSE = 180,
    LEFT90 = 270
}

public static class RotateAngleEnum
{
    public static RotateAngle RotateRight(this RotateAngle rotateAngle)
    {
        return rotateAngle switch
        {
            RotateAngle.ZERO => RotateAngle.RIGHT90,
            RotateAngle.RIGHT90 => RotateAngle.REVERSE,
            RotateAngle.REVERSE => RotateAngle.LEFT90,
            RotateAngle.LEFT90 => RotateAngle.ZERO,
            _ => throw new NotImplementedException(),
        };
    }

    public static RotateAngle RotateLeft(this RotateAngle rotateAngle)
    {
        return rotateAngle switch
        {
            RotateAngle.ZERO => RotateAngle.LEFT90,
            RotateAngle.RIGHT90 => RotateAngle.ZERO,
            RotateAngle.REVERSE => RotateAngle.RIGHT90,
            RotateAngle.LEFT90 => RotateAngle.REVERSE,
            _ => throw new NotImplementedException(),
        };
    }
}

