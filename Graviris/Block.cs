namespace Graviris;

public class Block
{
    private Dictionary<Direction, Block> connectBlocks;
    private Point location;
    private FieldController fieldController;
    private Color color;

    public Block(Point location, FieldController fieldController, Color color)
    {
        this.location = location;
        this.connectBlocks = new Dictionary<Direction, Block>();
        this.fieldController = fieldController;
        this.color = color;
    }

    public void Connect(Block block, Direction dir)
    {
        this.connectBlocks.Add(dir, block);
    }

    public void Disconnect(Direction dir)
    {
        if (this.connectBlocks.ContainsKey(dir))
        {
            this.connectBlocks.Remove(dir);
        }
    }

    public Point GetLocation()
    {
        return this.location;
    }

    public int GetX()
    {
        return this.location.X;
    }

    public int GetY()
    {
        return this.location.Y;
    }

    public Color GetColor()
    {
        return this.color;
    }

    public Dictionary<Direction, Block> GetConnectBlocks()
    {
        return this.connectBlocks;
    }

    public bool IsExistConnectBlock(Direction dir)
    {
        if (connectBlocks != null)
        {
            if (this.connectBlocks.ContainsKey(dir))
            {
                if (this.connectBlocks[dir] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Move(int x, int y)
    {
        this.location.Offset(x, y);
    }

    public bool IsEnableFall()
    {
        Direction gravityDirection = this.fieldController.GetGravityDirection();
        if (gravityDirection == Direction.Down)
        {
            if (this.location.Y < GlobalVariables.FIELD_HEIGHT_NUM - 1)
            {
                if (!this.fieldController.CheckBlockExist(Point.Add(this.location, new Size(0, 1))))
                {
                    return true;
                }
            }
        }
        else if (gravityDirection == Direction.Up)
        {
            if (this.location.Y > 0)
            {
                if (!this.fieldController.CheckBlockExist(Point.Add(this.location, new Size(0, -1))))
                {
                    return true;
                }
            }
        }


        return false;
    }
}