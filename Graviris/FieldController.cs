using System.Diagnostics;
using System.Linq;

namespace Graviris;

public class FieldController
{
    private List<Block> fieldBlockList;
    private Direction gravityDirection;

    public FieldController()
    {
        this.fieldBlockList = new List<Block>();
        this.gravityDirection = Direction.Down;
    }

    public List<Block> GenerateBlock(BlockShape shape, Point point, RotateAngle rotateAngle)
    {
        List<Block> blockList = new List<Block>();
        Size[] blockLocs = this.GetGenerateBlockLocations(shape, rotateAngle);
        Color color = Color.Blue;

        switch (shape)
        {
            case BlockShape.I:
                Direction connectDirI1 = Direction.Up.GetRotateDirection(rotateAngle);
                Direction connectDirI2 = Direction.Down.GetRotateDirection(rotateAngle);
                color = Color.Blue;
                Block blockI1 = new Block(point, this, color);
                Block blockI2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockI3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockI4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockI1, connectDirI1, blockI2);
                this.ConnectBlocks(blockI1, connectDirI2, blockI3);
                this.ConnectBlocks(blockI3, connectDirI2, blockI4);
                blockList.Add(blockI1);
                blockList.Add(blockI2);
                blockList.Add(blockI3);
                blockList.Add(blockI4);
                break;

            case BlockShape.O:
                color = Color.Red;
                Block blockO1 = new Block(point, this, color);
                Block blockO2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockO3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockO4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockO1, Direction.Right, blockO2);
                this.ConnectBlocks(blockO2, Direction.Down, blockO4);
                this.ConnectBlocks(blockO4, Direction.Left, blockO3);
                this.ConnectBlocks(blockO3, Direction.Up, blockO1);
                blockList.Add(blockO1);
                blockList.Add(blockO2);
                blockList.Add(blockO3);
                blockList.Add(blockO4);
                break;

            case BlockShape.T:
                Direction connectDirT1 = Direction.Left.GetRotateDirection(rotateAngle);
                Direction connectDirT2 = Direction.Up.GetRotateDirection(rotateAngle);
                Direction connectDirT3 = Direction.Right.GetRotateDirection(rotateAngle);
                color = Color.Orange;
                Block blockT1 = new Block(point, this, color);
                Block blockT2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockT3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockT4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockT1, connectDirT1, blockT2);
                this.ConnectBlocks(blockT1, connectDirT2, blockT3);
                this.ConnectBlocks(blockT1, connectDirT3, blockT4);
                blockList.Add(blockT1);
                blockList.Add(blockT2);
                blockList.Add(blockT3);
                blockList.Add(blockT4);
                break;

            case BlockShape.J:
                Direction connectDirJ1 = Direction.Up.GetRotateDirection(rotateAngle);
                Direction connectDirJ2 = Direction.Down.GetRotateDirection(rotateAngle);
                Direction connectDirJ3 = Direction.Left.GetRotateDirection(rotateAngle);
                color = Color.Yellow;
                Block blockJ1 = new Block(point, this, color);
                Block blockJ2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockJ3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockJ4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockJ1, connectDirJ1, blockJ2);
                this.ConnectBlocks(blockJ1, connectDirJ2, blockJ3);
                this.ConnectBlocks(blockJ3, connectDirJ3, blockJ4);
                blockList.Add(blockJ1);
                blockList.Add(blockJ2);
                blockList.Add(blockJ3);
                blockList.Add(blockJ4);
                break;

            case BlockShape.L:
                Direction connectDirL1 = Direction.Up.GetRotateDirection(rotateAngle);
                Direction connectDirL2 = Direction.Down.GetRotateDirection(rotateAngle);
                Direction connectDirL3 = Direction.Right.GetRotateDirection(rotateAngle);
                color = Color.Green;
                Block blockL1 = new Block(point, this, color);
                Block blockL2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockL3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockL4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockL1, connectDirL1, blockL2);
                this.ConnectBlocks(blockL1, connectDirL2, blockL3);
                this.ConnectBlocks(blockL3, connectDirL3, blockL4);
                blockList.Add(blockL1);
                blockList.Add(blockL2);
                blockList.Add(blockL3);
                blockList.Add(blockL4);
                break;

            case BlockShape.S:
                Direction connectDirS1 = Direction.Right.GetRotateDirection(rotateAngle);
                Direction connectDirS2 = Direction.Down.GetRotateDirection(rotateAngle);
                Direction connectDirS3 = Direction.Left.GetRotateDirection(rotateAngle);
                color = Color.BlueViolet;
                Block blockS1 = new Block(point, this, color);
                Block blockS2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockS3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockS4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockS1, connectDirS1, blockS2);
                this.ConnectBlocks(blockS1, connectDirS2, blockS3);
                this.ConnectBlocks(blockS3, connectDirS3, blockS4);
                blockList.Add(blockS1);
                blockList.Add(blockS2);
                blockList.Add(blockS3);
                blockList.Add(blockS4);
                break;

            case BlockShape.Z:
                Direction connectDirZ1 = Direction.Left.GetRotateDirection(rotateAngle);
                Direction connectDirZ2 = Direction.Down.GetRotateDirection(rotateAngle);
                Direction connectDirZ3 = Direction.Right.GetRotateDirection(rotateAngle);
                color = Color.DeepSkyBlue;
                Block blockZ1 = new Block(point, this, color);
                Block blockZ2 = new Block(Point.Add(point, blockLocs[0]), this, color);
                Block blockZ3 = new Block(Point.Add(point, blockLocs[1]), this, color);
                Block blockZ4 = new Block(Point.Add(point, blockLocs[2]), this, color);
                this.ConnectBlocks(blockZ1, connectDirZ1, blockZ2);
                this.ConnectBlocks(blockZ1, connectDirZ2, blockZ3);
                this.ConnectBlocks(blockZ3, connectDirZ3, blockZ4);
                blockList.Add(blockZ1);
                blockList.Add(blockZ2);
                blockList.Add(blockZ3);
                blockList.Add(blockZ4);
                break;
        }

        this.SetBlock(blockList);
        return blockList;
    }

    public Size[] GetGenerateBlockLocations(BlockShape shape, RotateAngle angle)
    {
        Size[] locations = new Size[3];

        Size loc0 = new Size();
        Size loc1 = new Size();
        Size loc2 = new Size();
        Size loc0Turn = new Size();
        Size loc1Turn = new Size();
        Size loc2Turn = new Size();

        switch (shape)
        {
            case BlockShape.I:
                loc0 = new Size(0, -1);
                loc1 = new Size(0, 1);
                loc2 = new Size(0, 2);
                loc0Turn = new Size(1, 0);
                loc1Turn = new Size(-1, 0);
                loc2Turn = new Size(-2, 0);
                break;
            case BlockShape.O:
                locations[0] = new Size(1, 0);
                locations[1] = new Size(0, 1);
                locations[2] = new Size(1, 1);
                return locations;
            case BlockShape.T:
                loc0 = new Size(-1, 0);
                loc1 = new Size(0, -1);
                loc2 = new Size(1, 0);
                loc0Turn = new Size(0, -1);
                loc1Turn = new Size(1, 0);
                loc2Turn = new Size(0, 1);
                break;
            case BlockShape.J:
                loc0 = new Size(0, -1);
                loc1 = new Size(0, 1);
                loc2 = new Size(-1, 1);
                loc0Turn = new Size(1, 0);
                loc1Turn = new Size(-1, 0);
                loc2Turn = new Size(-1, -1);
                break;
            case BlockShape.L:
                loc0 = new Size(0, -1);
                loc1 = new Size(0, 1);
                loc2 = new Size(1, 1);
                loc0Turn = new Size(1, 0);
                loc1Turn = new Size(-1, 0);
                loc2Turn = new Size(-1, 1);
                break;
            case BlockShape.S:
                loc0 = new Size(1, 0);
                loc1 = new Size(0, 1);
                loc2 = new Size(-1, 1);
                loc0Turn = new Size(0, 1);
                loc1Turn = new Size(-1, 0);
                loc2Turn = new Size(-1, -1);
                break;
            case BlockShape.Z:
                loc0 = new Size(-1, 0);
                loc1 = new Size(0, 1);
                loc2 = new Size(1, 1);
                loc0Turn = new Size(0, -1);
                loc1Turn = new Size(-1, 0);
                loc2Turn = new Size(-1, 1);
                break;
            default:
                break;
        }

        switch (angle)
        {
            case RotateAngle.ZERO:
                locations[0] = loc0;
                locations[1] = loc1;
                locations[2] = loc2;
                break;
            case RotateAngle.RIGHT90:
                locations[0] = loc0Turn;
                locations[1] = loc1Turn;
                locations[2] = loc2Turn;
                break;
            case RotateAngle.REVERSE:
                locations[0] = loc0 * -1;
                locations[1] = loc1 * -1;
                locations[2] = loc2 * -1;
                break;
            case RotateAngle.LEFT90:
                locations[0] = loc0Turn * -1;
                locations[1] = loc1Turn * -1;
                locations[2] = loc2Turn * -1;
                break;
            default:
                break;
        }

        return locations;
    }

    private void ConnectBlocks(Block baseBlock, Direction dir, Block connectedBlock)
    {
        baseBlock.Connect(connectedBlock, dir);
        connectedBlock.Connect(baseBlock, dir.GetReverseDirection());
    }

    private void SetBlock(List<Block> blockList)
    {
        foreach (Block block in blockList)
        {
            this.SetBlock(block);
        }
    }

    private void SetBlock(Block block)
    {
        this.fieldBlockList.Add(block);
    }

    public List<Block> GetBlocks()
    {
        return fieldBlockList;
    }

    public bool CheckBlockExist(Point location)
    {
        foreach (Block block in fieldBlockList)
        {
            if (location.Equals(block.GetLocation()))
            {
                return true;
            }
        }

        return false;
    }

    public void SortBlockList()
    {
        if (this.gravityDirection == Direction.Down)
        {
            BlockComparer comparer = new BlockComparer();
            this.fieldBlockList.Sort(comparer);
        }
        else if (this.gravityDirection == Direction.Up)
        {
            ReverseBlockComparer reverse = new ReverseBlockComparer();
            this.fieldBlockList.Sort(reverse);
        }
    }

    public bool EnableFallBlocksAll()
    {
        foreach (Block block in fieldBlockList)
        {
            if (this.EnableFallBlocks(block))
            {
                return true;
            }
        }

        return false;
    }

    public bool EnableFallBlocks(Block baseBlock)
    {
        //繋がったブロックを全てリスト化する
        List<Block> blockList = this.GetConnectBlocks(baseBlock);

        return this.EnableFallBlocks(blockList);
    }

    public bool EnableFallBlocks(List<Block> blockList)
    {
        //すべてのブロックが「落下可能」or「下に繋がったブロックがある」なら落下
        foreach (Block block in blockList)
        {
            if (!block.IsExistConnectBlock(this.gravityDirection) && !block.IsEnableFall())
            {
                return false; //落下できない
            }
        }

        //落下処理
        if (this.gravityDirection == Direction.Down)
        {
            this.MoveBlocks(blockList, 0, 1);
        }
        else if (this.gravityDirection == Direction.Up)
        {
            this.MoveBlocks(blockList, 0, -1);
        }

        return true;
    }

    public void MoveBlocks(List<Block> blockList, int x, int y)
    {
        foreach (Block block in blockList)
        {
            block.Move(x, y);
        }
    }

    private List<Block> GetConnectBlocks(Block block)
    {
        List<Block> blockList = new List<Block>();
        this.GetConnectBlocks(ref blockList, block);
        return blockList;
    }

    private void GetConnectBlocks(ref List<Block> blockList, Block block)
    {
        blockList.Add(block);
        Dictionary<Direction, Block> dict = block.GetConnectBlocks();
        foreach (Block connectBlock in dict.Values)
        {
            if (!blockList.Contains(connectBlock))
            {
                this.GetConnectBlocks(ref blockList, connectBlock);
            }
        }
    }

    public List<Block> GetFillLineBlocks(ref int fillLineNum)
    {
        fillLineNum = 0;
        Dictionary<int, List<Block>> dict = new Dictionary<int, List<Block>>();
        foreach (Block block in fieldBlockList)
        {
            int y = block.GetY();
            if (!dict.ContainsKey(y))
            {
                dict.Add(y, new List<Block>());
            }
            dict[y].Add(block);
        }

        List<Block> fillLineBlocks = new List<Block>();
        foreach (List<Block> list in dict.Values)
        {
            if (list.Count >= GlobalVariables.FIELD_WIDTH_NUM)
            {
                fillLineBlocks.AddRange(list);
                fillLineNum++;
            }
        }

        return fillLineBlocks;
    }

    public void DeleteBlock(List<Block> blockList)
    {
        foreach (Block block in blockList)
        {
            this.DeleteBlock(block);
        }
    }

    public void DeleteBlock(Block block)
    {
        Dictionary<Direction, Block> dict = block.GetConnectBlocks();
        foreach (Direction dir in dict.Keys)
        {
            Block connectBlock = dict[dir];
            connectBlock.Disconnect(dir.GetReverseDirection());
        }

        this.fieldBlockList.Remove(block);
    }

    public Direction GetGravityDirection()
    {
        return this.gravityDirection;
    }

    public void ReverseGravity()
    {
        this.gravityDirection = this.gravityDirection.GetReverseDirection();
    }
}

public class BlockComparer : IComparer<Block>
{
    public int Compare(Block? a, Block? b)
    {
        if (a == null || b == null)
        {
            throw new ArgumentNullException("値がnullです。");
        }

        int yCompare = b.GetY() - a.GetY();
        if (yCompare != 0)
        {
            return yCompare;
        }

        return b.GetX() - a.GetX();
    }
}

public class ReverseBlockComparer : IComparer<Block>
{
    public int Compare(Block? a, Block? b)
    {
        if (a == null || b == null)
        {
            throw new ArgumentNullException("値がnullです。");
        }

        int yCompare = a.GetY() - b.GetY();
        if (yCompare != 0)
        {
            return yCompare;
        }

        return a.GetX() -b.GetX();
    }
}