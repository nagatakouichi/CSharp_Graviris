namespace Graviris;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionEnum
{
    public static Direction GetReverseDirection(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new NotImplementedException(),
        };
    }

    public static Direction GetRotateDirection(this Direction direction, RotateAngle rotateAngle)
    => (direction, rotateAngle) switch
    {
        (Direction.Up, RotateAngle.ZERO) => Direction.Up,
        (Direction.Up, RotateAngle.RIGHT90) => Direction.Right,
        (Direction.Up, RotateAngle.REVERSE) => Direction.Down,
        (Direction.Up, RotateAngle.LEFT90) => Direction.Left,
        (Direction.Down, RotateAngle.ZERO) => Direction.Down,
        (Direction.Down, RotateAngle.RIGHT90) => Direction.Left,
        (Direction.Down, RotateAngle.REVERSE) => Direction.Up,
        (Direction.Down, RotateAngle.LEFT90) => Direction.Right,
        (Direction.Left, RotateAngle.ZERO) => Direction.Left,
        (Direction.Left, RotateAngle.RIGHT90) => Direction.Up,
        (Direction.Left, RotateAngle.REVERSE) => Direction.Right,
        (Direction.Left, RotateAngle.LEFT90) => Direction.Down,
        (Direction.Right, RotateAngle.ZERO) => Direction.Right,
        (Direction.Right, RotateAngle.RIGHT90) => Direction.Down,
        (Direction.Right, RotateAngle.REVERSE) => Direction.Left,
        (Direction.Right, RotateAngle.LEFT90) => Direction.Up,
        _ => throw new NotImplementedException(),
    };
}