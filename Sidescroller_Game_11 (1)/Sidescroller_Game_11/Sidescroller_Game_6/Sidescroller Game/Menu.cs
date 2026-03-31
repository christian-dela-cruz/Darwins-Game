//Menu.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Media;

namespace Sidescroller_Game
{
    internal class Menu : Form
    {
        protected internal SoundPlayer[] radio = new SoundPlayer[8];

        private Button start = new Button();
        private Button leaderBoards = new Button();
        private Button exit = new Button();
        private PictureBox logo = new PictureBox();
        private PictureBox soundOptons = new PictureBox();
        public string path;
        private bool isMusicPlaying = true;

        public Menu()
        {
            //---------------------------------------------------------------------------------
            //MainMENU
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Darwin's Game";
            try
            {
                path = Utility.DirManipulation.PathToAsset(@"menuBG.png");
                this.BackgroundImage = Image.FromFile(path);

            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
            }
            this.BackgroundImageLayout = ImageLayout.Stretch;
            int buttonwidth = 150;
            int buttonheight = 80;
            int positionX = (this.Width - buttonwidth) / 2;

            //---------------------------------------------------------------------------------
            //LOGO
            this.Controls.Add(logo);
            try
            {
                this.logo.Image = Image.FromFile(Utility.DirManipulation.PathToAsset("LOGO.gif"));

            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
            }
            this.logo.Width = buttonwidth * 3;
            this.logo.Height = buttonheight * 2;
            this.logo.BackgroundImageLayout = ImageLayout.Stretch;
            this.logo.BackColor = Color.Transparent;
            //---------------------------------------------------------------------------------
            //START
            this.Controls.Add(start);
            try
            {
                path = Utility.DirManipulation.PathToAsset(@"start.png");
                this.start.Image = Image.FromFile(path);
            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
            }
            this.start.Size = new Size(buttonwidth, buttonheight);
            this.start.Click += new EventHandler(gameStart);
            this.start.MouseHover += new EventHandler(startHover);
            this.start.MouseLeave += new EventHandler(startLeave);
            //---------------------------------------------------------------------------------
            //LEADERBOARDS
            this.Controls.Add(leaderBoards);
            try
            {
                path = Utility.DirManipulation.PathToAsset(@"leaderboards.png");
                this.leaderBoards.Image = Image.FromFile(path);
            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message}  \n  {e.StackTrace}"); Console.ResetColor();
            }
            this.leaderBoards.Size = new Size(buttonwidth, buttonheight);
            this.leaderBoards.Click += new EventHandler(gameLeaderboards);
            this.leaderBoards.MouseHover += new EventHandler(leaderboardsHover);
            this.leaderBoards.MouseLeave += new EventHandler(leaderboardsLeave);
            //---------------------------------------------------------------------------------
            //EXIT
            this.Controls.Add(exit);
            try
            {
                path = Utility.DirManipulation.PathToAsset(@"exit.png");
                this.exit.Image = Image.FromFile(path);
            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
            }
            this.exit.Size = new Size(buttonwidth, buttonheight);
            this.exit.Click += new EventHandler(gameExit);
            this.exit.MouseHover += new EventHandler(exitHover);
            this.exit.MouseLeave += new EventHandler(exitLeave);
            //---------------------------------------------------------------------------------
            //LOCATIONS
            this.start.Location = new Point(150, positionX);
            this.leaderBoards.Location = new Point(positionX, positionX);
            this.exit.Location = new Point(500, positionX);
            this.logo.Location = new Point(200, 100);
            //---------------------------------------------------------------------------------
            //MUSIC
            this.Controls.Add(soundOptons);

            this.soundOptons.Location = new Point(700, 10);
            this.soundOptons.Size = new Size(50, 50);
            this.soundOptons.BackgroundImageLayout = ImageLayout.Center;
            this.soundOptons.BackColor = Color.Transparent;

            try
            {
                path = Utility.DirManipulation.PathToAsset(@"soundIcon.png");
                this.soundOptons.Image = Image.FromFile(path);

            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
            }
            this.soundOptons.Click += new EventHandler(soundClick);

            string soundName;
            this.radio[0] = new SoundPlayer();
            soundName = @"\Sound\Little waves.wav";
            this.radio[0].SoundLocation = Utility.DirManipulation.PathToAsset(soundName);
            try { this.radio[0].PlayLooping(); } catch (FileNotFoundException e) { Console.WriteLine("{0}\nCannot find {1}.", e.Message, this.radio[0].SoundLocation); }

        }
        private void soundClick(object sender, EventArgs e)
        {
            isMusicPlaying = !isMusicPlaying;

            if (isMusicPlaying)
            {
                radio[0].PlayLooping();
            }
            else
            {
                radio[0].Stop();
            }

            UpdateSound();

        }
        private void UpdateSound()
        {
            string imagePath;

            if (isMusicPlaying)
            {
                imagePath = Utility.DirManipulation.PathToAsset(@"soundIcon.png");
                this.soundOptons.Image = Image.FromFile(imagePath);

            }
            else
            {
                imagePath = Utility.DirManipulation.PathToAsset(@"soundIcon_OFF.png"); 
                this.soundOptons.Image = Image.FromFile(imagePath);

            }

        }
        private void gameStart(object sender, EventArgs e)
        {
            this.Hide();
            Form stage1 = new Stage1(0, 0, 1, 100, 40, this.isMusicPlaying);
            stage1.ShowDialog();
            this.Show();
        }

        private void gameLeaderboards(object sender, EventArgs e)
        {
            new MenuForm().ShowDialog();
        }
        private void gameExit(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Exiting Game. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }
        //---------------------------------------------------------------------------------
        //HOVER

        private void startHover(object sender, EventArgs e)
        {
            try
            {
                path = Utility.DirManipulation.PathToAsset(@"start_hover.png");
                this.start.Image = Image.FromFile(path);
            }
            catch (FileNotFoundException f)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {f.Message} \n {f.StackTrace}"); Console.ResetColor();
            }
        }
        private void startLeave(object sender, EventArgs e)
        {
            path = Utility.DirManipulation.PathToAsset(@"start.png");
            this.start.Image = Image.FromFile(path);
        }
        private void leaderboardsHover(object sender, EventArgs e)
        {
            path = Utility.DirManipulation.PathToAsset(@"leaderboards_hover.png");
            this.leaderBoards.Image = Image.FromFile(path);
        }
        private void leaderboardsLeave(object sender, EventArgs e)
        {
            path = Utility.DirManipulation.PathToAsset(@"leaderboards.png");
            this.leaderBoards.Image = Image.FromFile(path);
        }
        private void exitHover(object sender, EventArgs e)
        {
            path = Utility.DirManipulation.PathToAsset(@"exit_hover.png");
            this.exit.Image = Image.FromFile(path);
        }
        private void exitLeave(object sender, EventArgs e)
        {
            path = Utility.DirManipulation.PathToAsset(@"exit.png");
            this.exit.Image = Image.FromFile(path);
        }


        public partial class MenuForm : Form
        {
            public string path;
            private PictureBox logobox = new PictureBox();
            private PictureBox namescore = new PictureBox();
            private GroupBox groupbox = new GroupBox();
            private TextBox player1 = new TextBox();
            private TextBox player2 = new TextBox ();
            private TextBox player3 = new TextBox();
            private TextBox player1Score = new TextBox ();
            private TextBox player2Score = new TextBox();
            private TextBox player3Score = new TextBox();

            public MenuForm()
            {
                int buttonwidth = 510;
                int buttonheight = 80;

                this.Size = new Size(buttonwidth, buttonheight * 5 - 30);
                this.Text = "Leaderboards";
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackColor = Color.White;

                //---------------------------------------------------------------------------------
                this.Controls.Add(logobox);
                try
                {
                    this.logobox.Image = Image.FromFile(Utility.DirManipulation.PathToAsset("LOGO.gif"));

                }
                catch (FileNotFoundException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
                }

                this.logobox.Width = buttonwidth;
                this.logobox.Height = buttonheight + 10;
                this.logobox.BackgroundImageLayout = ImageLayout.Stretch;
                this.logobox.BackColor = Color.White;
                this.logobox.Location = new Point(50, -10);
                //---------------------------------------------------------------------------------
                this.Controls.Add(namescore);
                try
                {
                    path = Utility.DirManipulation.PathToAsset(@"namescore.png");
                    this.namescore.Image = Image.FromFile(path);
                }
                catch (FileNotFoundException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
                }
                this.namescore.Width = buttonwidth;
                this.namescore.Height = buttonheight+2;
                this.namescore.BackgroundImageLayout = ImageLayout.Zoom;
                this.namescore.BackColor = Color.Black;
                this.namescore.Location = new Point(0,70);
                //---------------------------------------------------------------------------------
                //GroupBox
                this.Controls.Add(groupbox);
                this.groupbox.Hide();
                this.groupbox.Width = buttonwidth;
                this.groupbox.Height = buttonheight * 5;
                this.groupbox.BackColor = Color.White;
                this.groupbox.Location = new Point(0, 70);

                //---------------------------------------------------------------------------------
                //Player Text Box
                int playerWidth = 150, playerScoreWidth = 220;
                int playerHeight = 35;
                int playerRow = 12, playerScoreRow = 250;
                int column1 = 160, column2 = 210, column3 = 260;
                float fontSize = 15;
                //---------------------------------------------------------------------------------
                this.Controls.Add(player1);

                this.player1.Multiline = true;
                this.player1.ReadOnly = true;
                this.player1.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                this.player1.Size = new Size(playerWidth, playerHeight);
                this.player1.Location = new Point(playerRow, column1);
                this.player1.BackColor = Color.White;

                //---------------------------------------------------------------------------------
                this.Controls.Add(player2);

                this.player2.Multiline = true;
                this.player2.ReadOnly = true;
                this.player2.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                this.player2.Size = new Size(playerWidth, playerHeight);
                this.player2.Location = new Point(playerRow, column2);
                this.player2.BackColor = Color.White;

                //---------------------------------------------------------------------------------
                this.Controls.Add(player3);

                this.player3.Multiline = true;
                this.player3.ReadOnly = true;
                this.player3.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                this.player3.Size = new Size(playerWidth, playerHeight);
                this.player3.Location = new Point(playerRow, column3);
                this.player3.BackColor = Color.White;

                //---------------------------------------------------------------------------------
                this.Controls.Add(player1Score);

                this.player1Score.ReadOnly = true;
                this.player1Score.Multiline = true;
                this.player1Score.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                this.player1Score.Size = new Size(playerScoreWidth, playerHeight);
                this.player1Score.Location = new Point(playerScoreRow, column1);
                this.player1Score.BackColor = Color.DarkGray;
                //---------------------------------------------------------------------------------
                this.Controls.Add(player2Score);

                this.player2Score.ReadOnly = true;
                this.player2Score.Multiline = true;
                this.player2Score.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                this.player2Score.Size = new Size(playerScoreWidth, playerHeight);
                this.player2Score.Location = new Point(playerScoreRow, column2);
                this.player2Score.BackColor = Color.DarkGray;

                //---------------------------------------------------------------------------------
                this.Controls.Add(player3Score);

                this.player3Score.ReadOnly = true;
                this.player3Score.Multiline = true;
                this.player3Score.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
                this.player3Score.Size = new Size(playerScoreWidth, playerHeight);
                this.player3Score.Location = new Point(playerScoreRow, column3);
                this.player3Score.BackColor = Color.DarkGray;
            }
        }
    }
}

