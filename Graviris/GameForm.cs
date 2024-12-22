using System;
using System.Reflection;
namespace Graviris;

public class GameForm : Form
{
    private int GAMELOCATION_X = 50;
    private int GAMELOCATION_Y = 50;
    private int GAME_WIDTH = 250;
    private int GAME_HEIGHT = 500;
    private int BORDER_T = 5;
    private int BLOCK_WIDTH = 25;
    private String EXPLAIN_IMAGE_PATH = "image/ExplainImage.png";
    private String EXPLAIN_IMAGE_REVERSE_PATH = "image/ExplainImageReverse.png";
    private Panel gamePanel;
    private Panel reverseGaugePanel;
    private Label gameOverLabel;
    private Label deleteNumLabel;
    private PictureBox explainPicture;
    private FieldController fieldController;
    private GameController gameController;
    private Direction gravityDirection = Direction.Down;
    public GameForm()
    {
        this.fieldController = new FieldController();
        this.gameController = new GameController(this.fieldController);

        this.Text = "Graviris";
        this.ClientSize = new Size(350, 750);
        this.DoubleBuffered = true;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MinimizeBox = false;
        this.MaximizeBox = false;

        //ゲームパネル
        this.gamePanel = new Panel();
        this.gamePanel.Location = new Point(BORDER_T, BORDER_T);
        this.gamePanel.Size = new Size(GAME_WIDTH, GAME_HEIGHT);
        this.gamePanel.BackgroundImageLayout = ImageLayout.Zoom;
        this.gamePanel.BackgroundImage = Image.FromFile("image/GameBackGround.png");
        this.ControlDoubleBuffered(this.gamePanel, true);

        Panel borderPanel = new Panel();
        borderPanel.Location = new Point(GAMELOCATION_X - 5, GAMELOCATION_Y - 5);
        borderPanel.Size = new Size(GAME_WIDTH + BORDER_T * 2, GAME_HEIGHT + BORDER_T * 2);
        borderPanel.BackColor = Color.Black;

        //操作説明
        this.explainPicture = new PictureBox();
        explainPicture.BorderStyle = BorderStyle.FixedSingle;   //境界線
        explainPicture.Location = new Point(10, 550);
        explainPicture.Image = Image.FromFile(this.EXPLAIN_IMAGE_PATH);
        explainPicture.BackgroundImageLayout = ImageLayout.Stretch;
        explainPicture.SizeMode = PictureBoxSizeMode.StretchImage;
        explainPicture.Size = new Size(this.ClientSize.Width - 20, (int)(this.ClientSize.Width * 2 / 5));

        //REVERSEゲージ
        this.reverseGaugePanel = new Panel();
        reverseGaugePanel.BorderStyle = BorderStyle.FixedSingle;   //境界線
        reverseGaugePanel.Location = new Point(GAMELOCATION_X - 40, GAMELOCATION_Y);
        reverseGaugePanel.Size = new Size(30, 250);
        reverseGaugePanel.BackColor = Color.Gray;
        this.ControlDoubleBuffered(this.reverseGaugePanel, true);

        //消したライン数
        this.deleteNumLabel = new Label();
        deleteNumLabel.Text = "消したライン数:0";
        deleteNumLabel.Font = new Font("SanSerif", 10);
        deleteNumLabel.AutoSize = true;
        deleteNumLabel.Location = new Point(GAMELOCATION_X + GAME_WIDTH - 110, GAMELOCATION_Y - this.deleteNumLabel.Height);

        //ゲームオーバー
        this.gameOverLabel = new Label();
        this.gameOverLabel.Visible = false;
        this.gameOverLabel.Text = "Game Over";
        this.gameOverLabel.Font = new Font("SanSerif", 32);
        this.gameOverLabel.Size = new Size(GAME_WIDTH, 50);
        this.gameOverLabel.Location = new Point(GAMELOCATION_X + (GAME_WIDTH - this.gameOverLabel.Size.Width) / 2,
                                                GAMELOCATION_Y + (GAME_HEIGHT - this.gameOverLabel.Size.Height) / 2);

        this.Controls.Add(this.gameOverLabel);
        this.Controls.Add(borderPanel);
        this.Controls.Add(explainPicture);
        this.Controls.Add(reverseGaugePanel);
        this.Controls.Add(deleteNumLabel);

        borderPanel.Controls.Add(this.gamePanel);

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        timer.Interval = GlobalVariables.TIMER_INTERVAL;
        timer.Start();

        this.gamePanel.Paint += new PaintEventHandler(game_Paint);
        this.reverseGaugePanel.Paint += new PaintEventHandler(gauge_Paint);
        timer.Tick += new EventHandler(timer_Tick);
        this.KeyDown += new KeyEventHandler(game_KeyDown);
    }

    public void game_Paint(Object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        List<Block> blockList = this.fieldController.GetBlocks();

        foreach (Block block in blockList)
        {
            g.FillRectangle(new SolidBrush(block.GetColor()), block.GetX() * BLOCK_WIDTH, block.GetY() * BLOCK_WIDTH, BLOCK_WIDTH, BLOCK_WIDTH);
        }

        if (this.gameController.GetGameState() == GameState.GameOver)
        {
            this.gameOverLabel.Visible = true;
        }

        Direction recentGravityDirection = this.fieldController.GetGravityDirection();
        if (this.gravityDirection != recentGravityDirection)
        {
            this.gravityDirection = recentGravityDirection;
            if (gravityDirection == Direction.Down)
            {
                this.explainPicture.Image = Image.FromFile(EXPLAIN_IMAGE_PATH);
            }
            else if (gravityDirection == Direction.Up)
            {
                this.explainPicture.Image = Image.FromFile(EXPLAIN_IMAGE_REVERSE_PATH);
            }
        }
    }

    public void gauge_Paint(Object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        int reverseCounter = this.gameController.GetReverseCounter();
        int gauseHeight = this.reverseGaugePanel.Height / GlobalVariables.REVERSE_NUM;
        for (var i = 0; i < reverseCounter; i++)
        {
            g.FillRectangle(new SolidBrush(Color.Yellow), 0, gauseHeight * i, this.reverseGaugePanel.Width, gauseHeight);
        }
    }
    public void timer_Tick(Object sender, EventArgs e)
    {
        this.deleteNumLabel.Text = "消したライン数：" + this.gameController.GetDeleteLineNum();
        this.Refresh();
    }

    public void game_KeyDown(Object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.D:
            case Keys.Right:
                this.gameController.KeyDownRight();
                break;
            case Keys.A:
            case Keys.Left:
                this.gameController.KeyDownLeft();
                break;
            case Keys.S:
            case Keys.Down:
                this.gameController.KeyDownDown();
                break;
            case Keys.W:
            case Keys.Up:
                this.gameController.KeyDownUp();
                break;
            case Keys.E:
                this.gameController.RotateRight();
                break;
            case Keys.Q:
                this.gameController.RotateLeft();
                break;
            default:
                break;
        }

    }

    private void ControlDoubleBuffered(Control control, bool flag)
    {
        control.GetType().InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            control,
            new object[] { flag });
    }
}