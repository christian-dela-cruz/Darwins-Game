using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization.Formatters;

namespace Darwins_Game
{
    internal abstract class Entity : Panel
    {
        protected internal PictureBox visual = new PictureBox();
        private Image image = null;

        private bool keepAspectRatio;

        private int originalWidth;
        private int originalHeight;
        private double ratio;

        private Timer flipper;
        private byte states = 1;
        private byte state;
        public Entity(string fileName, bool keepAspectRatio = true)
        {
            this.Controls.Add(this.visual);

            // attempting to set value for image
            string path = Utility.DirManipulation.PathToAssets(fileName);
            this.image = Utility.ImgManipulation.SafeImportImage(path);
            this.keepAspectRatio = keepAspectRatio;

            Initialize();
        }
        internal protected void Initialize()
        {
            // update visual accordingly
            if (this.image != null)
            {
                this.visual.Image = image;
                this.ratio = ((double)image.Width / (double)image.Height);
                this.originalWidth = image.Width;
                this.originalHeight = image.Height;
                this.BackColor = Color.Transparent;

                this.visual.Size = this.image.Size;
                this.Size = this.visual.Size;
                this.visual.SizeMode = PictureBoxSizeMode.StretchImage;

                this.visual.SizeChanged += new EventHandler(this.Visual_SizeChanged);
            }
            else
            {
                this.visual = Utility.ImgManipulation.HandleNullImage(this.visual);
            }
        }
        internal protected void Visual_SizeChanged(object sender, EventArgs e)
        {
            if (this.visual.Image != null && keepAspectRatio)
            {
                this.visual.Size = Utility.ImgManipulation.Object_SizeChanged(ratio, this.visual.Width, this.visual.Height, originalWidth, originalHeight);
                this.originalWidth = this.visual.Width;
                this.originalHeight = this.visual.Height;
            }
        }
        protected internal Image Image
        { get { return this.image; } set { this.image = value; } }
        protected internal double Ratio
        { get { return this.ratio; } }
        protected internal int OriginalWidth
        { get { return this.originalWidth; } set { this.originalWidth = value; } }
        protected internal int OriginalHeight
        { get { return this.originalHeight; } set { this.originalHeight = value; } }
        protected internal bool KeepAspectRatio
        { get { return this.keepAspectRatio; } set { this.keepAspectRatio = value; } }
        protected internal Timer Flipper
        { get { return this.flipper; } set { this.flipper = value; } }
        protected internal byte States
        { get { return this.states; } set { this.states = value; } }
        protected internal byte State
        { get { return this.state; } set { this.state = value; } }
    }
    internal class Obstacle: Entity
    {
        private bool moving = false;
        
        public Obstacle(string fileName, byte states = 1, int padx = 0, int pady = 0): base(fileName)
        {
            this.States = states;
        }
        protected internal void ResizeWidth(int newWidth, int pad = 0)
        {
            this.Width = newWidth;
            this.visual.Width = newWidth * States;
            this.Height = this.visual.Height;
        }
        public bool Moving
        { get { return moving; } set {  moving = value; } }
        protected internal Rectangle HitBox
        { get { return this.visual.Bounds; } }
    }
    internal class Character: Entity
    {
        private bool jumping = false;
        private int jumpSpeed = 0;
        private int force = 12;

        public Character(string fileName): base(fileName)
        {
            this.State = 0;
            this.States = 2;
            ResizeWidth(this.Width/this.States);
            this.Flipper = new Timer();
            this.Flipper.Interval = 250;
            this.Flipper.Tick += Flipper_Tick;
            this.Flipper.Start();
            this.Paint += Control_Paint;
        }

        protected internal void Flipper_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < this.States; i++)
            {
                if (i == this.State)
                {
                    this.visual.Left = i * -this.Width;
                    this.State++;
                    if (this.State == this.States)
                    {
                        this.State = 0;
                    }
                    break;
                }
            }
        }
        protected internal void ResizeWidth(int newWidth)
        {
            this.Width = newWidth;
            this.visual.Width = newWidth*this.States;
            this.Height = this.visual.Height;
        }
        protected internal void Freeze()
        {
            this.Flipper.Stop();
        }
        protected internal void Sprint()
        {
            this.Flipper.Start();
        }
        protected internal void Control_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            Rectangle rec = this.Bounds;
            pe.Graphics.DrawRectangle(new Pen(Color.Red, 5f), rec);
        }
        protected internal bool Jumping
        { get { return this.jumping; } set {  this.jumping = value; } }
        protected internal int JumpSpeed
        { get { return this.jumpSpeed;  } set { this.jumpSpeed = value; } }
        protected internal int Force
        { get { return this.force; } set {  this.force = value; } }
    }
    internal class Platform : Entity
    {
        public Platform(string fileName) : base(fileName)
        {
            this.visual.BackgroundImage = this.visual.Image;
            this.BackgroundImageLayout = ImageLayout.Tile;
            this.visual.Dock = DockStyle.Fill;
            this.visual.Image = null;
        }
    }
}
