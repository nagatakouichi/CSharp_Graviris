namespace Graviris;

public class GameController
{
    private GameState gameState = GameState.Start;
    private FieldController fieldController;
    private List<Block> operatingBlocks;
    private BlockShape operatingShape;
    private RotateAngle operatingAngle;
    private int counter = 0;
    private int GAMESPEED = 30;
    private Random random;
    private List<BlockShape> createShapeList;
    private int waitCounter = 0;
    private int reverseCounter = 0;
    private int deleteLineNum = 0;

    public GameController(FieldController fieldController)
    {
        this.fieldController = fieldController;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        timer.Interval = GlobalVariables.TIMER_INTERVAL;
        timer.Start();

        gameState = GameState.BlockGeneration;
        random = new Random();
        createShapeList = new List<BlockShape>();

        timer.Tick += new EventHandler(timer_Tick);
    }

    public void timer_Tick(Object sender, EventArgs e)
    {
        if (waitCounter > 0)
        {
            waitCounter--;
            return;
        }

        Direction gravityDirection = this.fieldController.GetGravityDirection();

        switch (this.gameState)
        {
            case GameState.BlockGeneration:
                if (createShapeList.Count <= 0)
                {
                    this.createShapeList = new List<BlockShape>(this.CreateRandomShapeList());
                }
                this.operatingShape = this.createShapeList[0];
                this.createShapeList.RemoveAt(0);
                Point baseLoc = new Point();
                int dy = 0;

                if (gravityDirection == Direction.Down)
                {
                    this.operatingAngle = RotateAngle.ZERO;
                    baseLoc = new Point(4, 1);
                    dy = -1;
                }
                else if (gravityDirection == Direction.Up)
                {
                    this.operatingAngle = RotateAngle.REVERSE;
                    baseLoc = new Point(4, GlobalVariables.FIELD_HEIGHT_NUM - 2);
                    dy = 1;
                }

                //生成位置に既にブロックがある場合の処理
                bool isOverlap = false;
                do
                {
                    isOverlap = false;
                    Point[] generateLocs = this.GetGenerateBlockLocations(baseLoc, this.operatingShape, this.operatingAngle);
                    foreach (Point loc in generateLocs)
                    {
                        if (this.fieldController.CheckBlockExist(loc))
                        {
                            isOverlap = true;
                            baseLoc.Offset(0, dy);
                            break;
                        }
                    }
                } while (isOverlap);

                this.operatingBlocks = this.fieldController.GenerateBlock(this.operatingShape, baseLoc, this.operatingAngle);

                this.gameState = GameState.BlockOperation;
                this.CheckGameOver();
                break;

            case GameState.BlockOperation:
                if (counter < this.GAMESPEED)
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                    if (!this.fieldController.EnableFallBlocks(this.operatingBlocks))
                    {
                        this.gameState = GameState.CheckLine;
                    }
                }
                break;

            case GameState.CheckLine:
                int fillLineNum = 0;
                List<Block> fillLineBlocks = this.fieldController.GetFillLineBlocks(ref fillLineNum);
                if (fillLineBlocks.Count > 0)
                {
                    this.fieldController.DeleteBlock(fillLineBlocks);
                    this.gameState = GameState.WaitFall;
                    this.deleteLineNum = this.deleteLineNum + fillLineNum;
                    this.reverseCounter = this.reverseCounter + fillLineNum;

                    if (this.reverseCounter >= GlobalVariables.REVERSE_NUM)
                    {
                        this.fieldController.ReverseGravity();
                    }
                }
                else
                {
                    this.gameState = GameState.BlockGeneration;
                }
                break;

            case GameState.WaitFall:
                this.fieldController.SortBlockList();
                if (!this.fieldController.EnableFallBlocksAll())
                {
                    waitCounter = 10;
                    this.gameState = GameState.CheckLine;
                    if (this.reverseCounter >= GlobalVariables.REVERSE_NUM)
                    {
                        this.reverseCounter = 0;
                    }
                }
                break;
            default:
                break;
        }
    }

    public GameState GetGameState()
    {
        return this.gameState;
    }

    public int GetReverseCounter()
    {
        return this.reverseCounter;
    }

    public int GetDeleteLineNum()
    {
        return this.deleteLineNum;
    }

    private BlockShape[] CreateRandomShapeList()
    {
        List<BlockShape> blockShapes = Enum.GetValues(typeof(BlockShape)).Cast<BlockShape>().ToList();
        BlockShape[] randomList = new BlockShape[blockShapes.Count];

        for (var i = 0; i < randomList.Length; i++)
        {
            int pos = random.Next(0, randomList.Length - i - 1);
            randomList[i] = blockShapes[pos];
            blockShapes.RemoveAt(pos);
        }

        return randomList;
    }

    public void KeyDownRight()
    {
        if (this.gameState == GameState.BlockOperation)
        {
            if (this.operatingBlocks != null)
            {
                //移動可能チェック　右に繋がったブロックがある　or　（右端でない　and 右にブロックがない）
                //移動不可の場合止める処理のため、逆のifチェックを行う
                foreach (Block block in this.operatingBlocks)
                {
                    if (!block.IsExistConnectBlock(Direction.Right))
                    {
                        if (block.GetX() >= GlobalVariables.FIELD_WIDTH_NUM - 1
                        || this.fieldController.CheckBlockExist(Point.Add(block.GetLocation(), new Size(1, 0))))
                        {
                            //移動不可
                            return;
                        }
                    }
                }

                //すべて移動可能なら移動させる
                this.fieldController.MoveBlocks(this.operatingBlocks, 1, 0);
            }
        }
    }

    public void KeyDownLeft()
    {
        if (this.gameState == GameState.BlockOperation)
        {
            if (this.operatingBlocks != null)
            {
                foreach (Block block in this.operatingBlocks)
                {
                    if (!block.IsExistConnectBlock(Direction.Left))
                    {
                        if (block.GetX() <= 0
                        || this.fieldController.CheckBlockExist(Point.Add(block.GetLocation(), new Size(-1, 0))))
                        {
                            //移動不可
                            return;
                        }
                    }
                }

                //すべて移動可能なら移動させる
                this.fieldController.MoveBlocks(this.operatingBlocks, -1, 0);
            }
        }
    }

    public void KeyDownDown()
    {
        if (this.gameState == GameState.BlockOperation)
        {
            Direction gravityDirection = this.fieldController.GetGravityDirection();
            if (gravityDirection == Direction.Down)
            {
                this.FallOperatingBlocks();
            }
            else if (gravityDirection == Direction.Up)
            {
                this.WarpOperatingBlocks();
            }
        }
    }

    public void KeyDownUp()
    {
        if (this.gameState == GameState.BlockOperation)
        {
            Direction gravityDirection = this.fieldController.GetGravityDirection();
            if (gravityDirection == Direction.Down)
            {
                this.WarpOperatingBlocks();
            }
            else if (gravityDirection == Direction.Up)
            {
                this.FallOperatingBlocks();
            }


        }
    }

    private void FallOperatingBlocks()
    {
        if (this.operatingBlocks != null)
        {
            this.fieldController.EnableFallBlocks(this.operatingBlocks);
        }
    }

    private void WarpOperatingBlocks()
    {
        if (this.operatingBlocks != null)
        {
            bool isFall = true;
            while (isFall)
            {
                isFall = this.fieldController.EnableFallBlocks(this.operatingBlocks);
            }
        }
    }

    public void RotateRight()
    {
        if (this.gameState == GameState.BlockOperation)
        {
            this.RotateOperatingBlocks(this.operatingAngle.RotateRight());
        }
    }

    public void RotateLeft()
    {
        if (this.gameState == GameState.BlockOperation)
        {
            this.RotateOperatingBlocks(this.operatingAngle.RotateLeft());
        }
    }

    public void RotateOperatingBlocks(RotateAngle rotatedAngle)
    {
        if (this.operatingBlocks != null)
        {
            //回転後の位置を予測
            Point baseLoc = this.operatingBlocks[0].GetLocation();
            Point[] rotateLocs = this.GetGenerateBlockLocations(baseLoc, this.operatingShape, rotatedAngle);

            List<Point> overlapLocs = new List<Point>();
            //移動は2回まで
            for (var count = 0; count < 2; count++)
            {
                //重複位置を取得 回転させるブロックとの重複は無視する　また、画面外になるブロックも取得
                overlapLocs = this.GetOverlapLocationsAfterRotate(rotateLocs);
                overlapLocs.AddRange(this.GetOutsideLocations(rotateLocs));
                if (overlapLocs.Count <= 0)
                {
                    break;
                }
                //重複位置をもとに予測位置をずらす
                Size moveSize = new Size(0, 0);
                Point overlapLoc = overlapLocs[0];
                if (baseLoc.X < overlapLoc.X)
                {
                    moveSize = new Size(-1, 0);
                }
                else if (baseLoc.X > overlapLoc.X)
                {
                    moveSize = new Size(1, 0);
                }
                else if (baseLoc.Y < overlapLoc.Y)
                {
                    moveSize = new Size(0, 1);
                }
                else if (baseLoc.Y > overlapLoc.Y)
                {
                    moveSize = new Size(0, -1);
                }

                for (var i = 0; i < rotateLocs.Length; i++)
                {
                    rotateLocs[i] = Point.Add(rotateLocs[i], moveSize);
                }
            }

            //ずらした後重複、画面外チェック　2回ずらしても重なっているなら回転させない
            overlapLocs = this.GetOverlapLocationsAfterRotate(rotateLocs);
            overlapLocs.AddRange(this.GetOutsideLocations(rotateLocs));
            if (overlapLocs.Count <= 0)
            {
                //回転と位置ずらしをして生成
                this.fieldController.DeleteBlock(this.operatingBlocks);
                this.operatingAngle = rotatedAngle;
                this.operatingBlocks = this.fieldController.GenerateBlock(this.operatingShape, rotateLocs[0], this.operatingAngle);
            }
        }
    }

    private Point[] GetGenerateBlockLocations(Point baseLoc, BlockShape shape, RotateAngle angle)
    {
        Size[] generateMove = this.fieldController.GetGenerateBlockLocations(shape, angle);
        Point[] generateLocs = new Point[4];
        generateLocs[0] = baseLoc;
        for (var i = 0; i < 3; i++)
        {
            generateLocs[i + 1] = Point.Add(baseLoc, generateMove[i]);
        }

        return generateLocs;
    }

    private List<Point> GetOverlapLocationsAfterRotate(Point[] rotateLocs)
    {
        List<Point> overlapLocs = new List<Point>();
        foreach (Point loc in rotateLocs)
        {
            if (!this.IsOverlapOperatingBlocks(loc) && this.fieldController.CheckBlockExist(loc))
            {
                overlapLocs.Add(loc);
            }
        }

        return overlapLocs;
    }

    private bool IsOverlapOperatingBlocks(Point loc)
    {
        if (this.operatingBlocks != null)
        {
            foreach (Block block in this.operatingBlocks)
            {
                if (block.GetLocation() == loc)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private List<Point> GetOutsideLocations(Point[] locs)
    {
        List<Point> outsideLocs = new List<Point>();
        foreach (Point point in locs)
        {
            if (point.X >= GlobalVariables.FIELD_WIDTH_NUM
            || point.X < 0
            || point.Y >= GlobalVariables.FIELD_HEIGHT_NUM
            || point.Y < 0)
            {
                outsideLocs.Add(point);
            }
        }

        return outsideLocs;
    }

    private void CheckGameOver()
    {
        foreach (Block block in this.fieldController.GetBlocks())
        {
            Point loc = block.GetLocation();
            if (loc.Y < 0 || loc.Y >= GlobalVariables.FIELD_HEIGHT_NUM)
            {
                this.gameState = GameState.GameOver;
                Console.WriteLine("GameOver");
                break;
            }
        }
    }
}

public enum GameState
{
    Start,
    BlockGeneration,
    BlockOperation,
    CheckLine,
    WaitFall,
    GameOver
}