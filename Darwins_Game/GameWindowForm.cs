using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Media;
using static Sidescroller_Game.Menu;

namespace Sidescroller_Game
{
    internal abstract class GameWindowForm : Form
    {
        // Main background control visual and container.
        // Important to note that this.Height returns a value that includes the height of the toolbar.
        // A picturebox that has dockstyle set to fill can return a height value without the height of the toolbar.
        // This is useful in placing other controls later on.
        protected internal PictureBox gameBox = new PictureBox();

        // This control represents the platform on which the character runs on and jumps from.
        protected internal Platform terrain;

        // The control representing the character
        protected internal Character character;

        // Array containing obstacles to be queued
        protected internal Obstacle[] obstacles;
        protected internal Obstacle[] backObjects;
        protected internal sbyte[][] moveFlags;

        // Soundplayers
        protected internal SoundPlayer[] radio = new SoundPlayer[8];
        private bool playMusic;


        // Timers
        protected internal Timer mainTimer;
        protected internal Timer obstacleQueuer;
        protected internal Timer backObjectQueuer;
        private int gameInterval;

        // Score tracker
        private int score = 0;
        private int distance = 0;
        private int distanceWeight = 1;
        private int endDistance = 100;

        // Panel Prompt
        private Panel prompt;
        private Label messageHead;
        private Label messageBody;
        private TextBox enterName;

        // Some variables to help with processing
        private int baseHeight;

        protected internal int Score
        { get { return this.score; } }
        protected internal int Distance
        { get { return this.distance; } }
        protected internal int DistanceWeight
        { get { return this.distanceWeight; } set { this.distanceWeight = value; } }
        protected internal int EndDistance
        { get { return this.endDistance; } set { this.endDistance = value; } }
        protected internal int BaseHeight
        { get { return this.baseHeight; } set { this.baseHeight = value; } }
        protected internal int GameInterval
        { get { return this.gameInterval; } }
        protected internal bool PlayMusic
        { get { return this.playMusic; } }

        public GameWindowForm(int score = 0, int distance = 0, int distanceWeight = 1, int endDistance = 50, int gameInterval = 40, bool playMusic = true)
        {
            // Formatting the Form itself ==================================================
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(960, 540);
            this.Text = string.Format("Sidescroller Game {0}x{1}", this.Width, this.Height);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.Green;

            this.score = score;
            this.distance = distance;
            this.distanceWeight = distanceWeight;
            this.endDistance = endDistance;
            this.gameInterval = gameInterval;
            this.playMusic = playMusic;

            Initialize();
            this.endDistance = endDistance;
        }
        protected internal void Initialize()
        {
            string path;
            Image image;

            // Formatting the gameBox ======================================================
            path = Utility.DirManipulation.PathToAssets(@"assets\STAGE BGS\pixilart-drawing.gif");
            image = Utility.ImgManipulation.SafeImportImage(path);
            if (image != null)
            {
                //this.gameBox.BackgroundImage = image;
                this.gameBox.BackColor = Color.White;
                this.gameBox.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            { this.gameBox = Utility.ImgManipulation.HandleNullImage(this.gameBox); }

            this.gameBox.Dock = DockStyle.Fill;
            this.Controls.Add(this.gameBox);

            // Formatting the terrain =====================================================
            this.terrain = new Platform(@"\assets\sand.jpg");
            this.terrain.KeepAspectRatio = false;
            this.terrain.Height = 50;
            this.terrain.Top = this.gameBox.Height - this.terrain.Height;
            this.terrain.Width = (int)System.Math.Ceiling((decimal)this.gameBox.Width * 2 / (decimal)this.terrain.Width) * this.terrain.Width;
            this.terrain.visual.BackgroundImage = null;
            this.terrain.BackColor = Color.Black;

            this.gameBox.Controls.Add(this.terrain);

            // Formatting back objects ====================================================
            this.backObjects = new Obstacle[4];
            this.moveFlags = new sbyte[0][];
            this.backObjects[0] = new Obstacle(@"\assets\Obstacles\cloud1.png");
            this.backObjects[0].ResizeWidth(100);
            this.backObjects[1] = new Obstacle(@"\assets\Obstacles\cloud2.png");
            this.backObjects[1].ResizeWidth(100);
            this.backObjects[2] = new Obstacle(@"\assets\Obstacles\cloud3.png");
            this.backObjects[2].ResizeWidth(40);
            this.backObjects[3] = new Obstacle(@"\assets\Obstacles\cloud3.png");
            this.backObjects[3].ResizeWidth(60);

            for (int i = 0; i < backObjects.Length; i++)
            {
                if (backObjects[i] != null)
                {
                    this.gameBox.Controls.Add(this.backObjects[i]);
                    this.backObjects[i].Left = this.gameBox.Width;
                }
            }

            // Formatting obstacles =======================================================
            this.obstacles = new Obstacle[4];
            for (int i = 0; i < this.obstacles.Length; i++)
            {
                this.obstacles[i] = new Obstacle(@"\assets\Obstacles\rock.png");
                this.gameBox.Controls.Add(this.obstacles[i]);
                this.obstacles[i].ResizeWidth(100);
                Size size = this.obstacles[i].Size;
                this.obstacles[i].Left = this.gameBox.Width;
                this.obstacles[i].Top = this.gameBox.Height - size.Height - this.terrain.Height;
            }

            // Initializting moveFlags ====================================================
            this.moveFlags = new sbyte[2][];
            this.moveFlags[0] = new sbyte[this.backObjects.Length];
            this.moveFlags[1] = new sbyte[this.obstacles.Length];
            for (int i = 0; i < this.moveFlags[0].Length; i++) { this.moveFlags[0][i] = (sbyte)i; }
            for (int i = 0; i < this.moveFlags[1].Length; i++) { this.moveFlags[0][i] = (sbyte)i; }

            // Formatting the character ===================================================
            this.character = new Character(@"\assets\STAGE 01\missinglinkv2.png");
            this.character.ResizeWidth(125);
            this.character.Location = new Point(100, this.gameBox.Height - this.terrain.Height - this.character.Height);
            this.baseHeight = this.character.Top;
            this.gameBox.Controls.Add(this.character);

            // Formatting soundplayers ====================================================
            if (this.playMusic)
            {
                string soundName;
                this.radio[0] = new SoundPlayer();
                soundName = @"\Sound\Little waves.wav";
                this.radio[0].SoundLocation = Utility.DirManipulation.PathToAsset(soundName);
                try { this.radio[0].PlayLooping(); } catch (FileNotFoundException e) { Console.WriteLine("{0}\nCannot find {1}.", e.Message, this.radio[0].SoundLocation); }

                this.radio[1] = new SoundPlayer();
                soundName = @"\Sound\Achieve.wav";
                this.radio[1].SoundLocation = Utility.DirManipulation.PathToAsset(soundName);

                this.radio[2] = new SoundPlayer();
                soundName = @"\Sound\Defeat.wav";
                this.radio[2].SoundLocation = Utility.DirManipulation.PathToAsset(soundName);
            }

            // Formatting the timers ======================================================
            if (this.gameInterval < 16)
            { this.gameInterval = 16; }
            this.mainTimer = new Timer();
            this.mainTimer.Tick += new EventHandler(MainTimer_Tick);
            this.mainTimer.Interval = 40;
            this.mainTimer.Start();

            this.obstacleQueuer = new Timer();
            this.obstacleQueuer.Tick += ObstacleQueuer_Tick;
            this.obstacleQueuer.Interval = this.mainTimer.Interval * gameInterval;
            this.obstacleQueuer.Start();

            this.backObjectQueuer = new Timer();
            this.backObjectQueuer.Tick += BackObjectQueuer_Tick;
            this.backObjectQueuer.Interval = this.mainTimer.Interval * 100;
            this.backObjectQueuer.Start();

            this.KeyDown += new KeyEventHandler(this.keyisdown);
            this.KeyUp += new KeyEventHandler(this.keyisup);

            // Formatting the prompt ======================================================
            this.prompt = new Panel();
            this.prompt.BackColor = Color.Beige;
            this.prompt.BorderStyle = BorderStyle.Fixed3D;
            this.prompt.Size = new Size(this.gameBox.Width * 3 / 5, this.gameBox.Height * 3 / 5);
            this.prompt.Location = new Point(this.gameBox.Width / 5, this.gameBox.Height / 5);
            this.prompt.Padding = new Padding(20, 100, 20, 0);

            this.messageHead = new Label();
            this.messageBody = new Label();
            this.enterName = new TextBox();


            this.messageHead.Height = 50;
            this.messageBody.Height = 100;

            this.prompt.Controls.Add(this.messageBody);
            this.prompt.Controls.Add(this.messageHead);
            this.prompt.Controls.Add(this.enterName);

            this.messageHead.Font = new Font(FontFamily.GenericSerif, 20f, FontStyle.Bold);
            this.messageBody.Font = new Font(FontFamily.GenericMonospace, 15f, FontStyle.Regular);
            this.enterName.Font = new Font(enterName.Font.FontFamily, 20f, FontStyle.Regular);
            this.messageHead.Dock = DockStyle.Top;
            this.messageBody.Dock = DockStyle.Top;
            this.enterName.Dock = DockStyle.Bottom;
            this.enterName.Visible = false;

            this.messageHead.TextAlign = ContentAlignment.TopCenter;
            this.messageBody.TextAlign = ContentAlignment.TopCenter;
            this.enterName.TextAlign = HorizontalAlignment.Center;

            this.gameBox.Controls.Add(this.prompt);
            this.prompt.Visible = false;
            this.prompt.BringToFront();
        }

        private void BackObjectQueuer_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int rand;
            int maxIndex = this.backObjects.Length;
            while (maxIndex > 0)
            {
                rand = r.Next(0, maxIndex);
                if (backObjects[rand].Moving)
                {
                    this.moveFlags[0][maxIndex - 1] += this.moveFlags[0][rand];
                    this.moveFlags[0][rand] = Convert.ToSByte(this.moveFlags[0][maxIndex - 1] - this.moveFlags[0][rand]);
                    this.moveFlags[0][maxIndex - 1] -= this.moveFlags[0][rand];
                    maxIndex--;
                }
                else
                {
                    this.backObjects[rand].Moving = true;
                    this.backObjects[rand].Top = r.Next(25, 150);
                    for (int i = 0; i < this.moveFlags[0].Length; i++) { this.moveFlags[0][i] = (sbyte)i; }
                    break;
                }
            }
        }
        private void ObstacleQueuer_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int rand;
            int maxIndex = this.obstacles.Length;
            while (maxIndex > 0)
            {
                rand = r.Next(0, maxIndex);
                if (obstacles[rand].Moving)
                {
                    this.moveFlags[1][maxIndex - 1] += this.moveFlags[1][rand];
                    this.moveFlags[1][rand] = Convert.ToSByte(this.moveFlags[1][maxIndex - 1] - this.moveFlags[1][rand]);
                    this.moveFlags[1][maxIndex - 1] -= this.moveFlags[1][rand];
                    maxIndex--;
                }
                else
                {
                    this.obstacles[rand].Moving = true;
                    for (int i = 0; i < this.moveFlags[1].Length; i++) { this.moveFlags[1][i] = (sbyte)i; }
                    break;
                }
            }
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            this.character.Top += this.character.JumpSpeed;

            if (this.character.Jumping && this.character.Force < 0)
            {
                this.character.Jumping = false;
            }

            if (this.character.Jumping)
            {
                this.character.JumpSpeed = -12;
                this.character.Force -= 1;
                this.character.Freeze();
            }
            else
            {
                this.character.JumpSpeed = 12;
            }

            if (this.character.Top > this.baseHeight - 1 && !this.character.Jumping)
            {
                this.character.Force = 12;
                this.character.Top = this.baseHeight;
                this.character.JumpSpeed = 0;
                this.character.Sprint();
            }

            if (this.terrain.Location.X > -this.terrain.Width / 2)
            {
                this.terrain.Left -= 16;
            }
            else
            {
                this.terrain.Left = 0;
            }

            for (int i = 0; i < obstacles.Length; i++)
            {
                if (obstacles[i].Moving)
                {
                    this.obstacles[i].Left -= 16;
                    if (this.obstacles[i].Left < 0)
                    {
                        this.obstacles[i].Moving = false;
                        this.obstacles[i].Left = this.gameBox.Width;
                    }
                }
                if (character.visual.Image != null && this.character.Bounds.IntersectsWith(this.obstacles[i].Bounds))
                {
                    this.mainTimer.Stop();
                    this.obstacleQueuer.Stop();
                    this.backObjectQueuer.Stop();
                    this.character.Freeze();

                    this.score += this.distance * this.distanceWeight;

                    this.messageHead.ForeColor = Color.Red;
                    this.messageHead.Text = String.Format("DEFEAT");
                    this.messageBody.Text = String.Format("You scored {0} points.\nPress R to try again\nor L to log score.", this.score);

                    this.prompt.Visible = true;

                    this.KeyDown += new KeyEventHandler(this.keyisRetry);
                    this.KeyDown += new KeyEventHandler(this.keyisLOG);

                    //Button retry =new Button();
                    //this.Controls.Add(retry);
                    //retry.BackColor = Color.Red;
                    //retry.Font =new Font(FontFamily.GenericMonospace, 10f, FontStyle.Bold);
                    //retry.Text = String.Format("Retry?");
                    //defeatMessage.TextAlign = ContentAlignment.MiddleCenter;

                    for (int j = 0; j < this.radio.Length; j++) { if (radio[j] != null) { radio[j].Stop(); } }
                    if (this.playMusic) { this.radio[2].Play(); }
                }
            }
            for (int i = 0; i < backObjects.Length; i++)
            {
                if (backObjects[i].Moving)
                {
                    this.backObjects[i].Left -= 4;
                    if (this.backObjects[i].Left < 0)
                    {
                        this.backObjects[i].Moving = false;
                        this.backObjects[i].Left = this.gameBox.Width;
                    }
                }
            }
            this.distance++;
            if (this.distance > this.endDistance)
            {
                this.mainTimer.Stop();
                this.character.Freeze();
                this.score += this.endDistance * this.distanceWeight;

                this.messageHead.Text = String.Format("CONGRATULATIONS!");
                this.messageBody.Text = String.Format("You scored {0} points. \nPress ENTER to proceed to next stage\nor L to log score", this.score);

                this.prompt.Visible = true;

                this.KeyDown += new KeyEventHandler(this.keyisProceed);
                this.KeyDown += new KeyEventHandler(this.keyisLOG);


                for (int i = 0; i < this.radio.Length; i++) { if (radio[i] != null) { radio[i].Stop(); } }
                if ( this.playMusic) { this.radio[1].Play(); }
            }
        }
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.W || e.KeyCode == Keys.Up && this.character.Jumping == false)
            {
                this.character.Jumping = true;
            }
        }
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (this.character.Jumping == true)
            {
                this.character.Jumping = false;
            }
        }

        internal protected void keyisRetry(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                this.Close();
                this.Hide();
                new Stage1(0, 0, 1, 100, 40, this.playMusic).ShowDialog();
            }
        }
        internal protected void keyisLOG(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                this.enterName.Visible = true;
                this.enterName.Focus();
                this.enterName.KeyDown += new KeyEventHandler(this.keyisEnter);
            }
        }
        internal protected void keyisEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string name = this.enterName.Text;
                if (name.Length == 0)
                {
                    name = "New Player";
                }
                this.Hide();
                new Menu().ShowDialog();

                Leaderboards.Score newScore = new Leaderboards.Score(name, this.score);
                Leaderboards.WriteData(newScore);
            }
        }
        internal protected virtual void keyisProceed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                new Stage2(this.score, 0, this.distanceWeight, this.endDistance, this.gameInterval-4, this.playMusic).ShowDialog();
            }
        }

    }
}
