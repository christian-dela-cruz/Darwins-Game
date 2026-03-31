using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace Darwins_Game
{
    internal class Stage4 : GameWindowForm
    {
        public Stage4(int score, int distance, int distanceWeight, int endDistance, int gameInterval, bool playMusic) : base(score, distance, distanceWeight, endDistance, gameInterval, playMusic)
        {
            this.EndDistance = 10;

            this.character.Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"STAGE 04\monkey stage.png"));
            this.character.Initialize();
            this.character.States = 3;
            this.character.ResizeWidth(74);
            this.character.Top = this.gameBox.Height - this.terrain.Height - this.character.Height;
            this.BaseHeight = this.character.Top;

            this.obstacles[1].Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"Obstacles\new log small.png"));
            this.obstacles[1].Initialize();
            this.obstacles[1].ResizeWidth(100);
            this.obstacles[1].Top = 400;
            this.obstacles[2].Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"Obstacles\small snake.png"));
            this.obstacles[2].Initialize();
            this.obstacles[2].ResizeWidth(100);
            this.obstacles[2].Top = 400;


        }
        internal protected override void keyisProceed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Console.WriteLine(this.GameInterval);
                this.Close();
                new Stage5(this.Score, 0, this.DistanceWeight, this.EndDistance, this.GameInterval - 4, this.PlayMusic).ShowDialog();
            }
        }
    }

}