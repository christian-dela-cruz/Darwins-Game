using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sidescroller_Game.Leaderboards;

namespace Sidescroller_Game
{    
    internal class Stage2: GameWindowForm
    {
        public Stage2(int score, int distance, int distanceWeight, int endDistance, int gameInterval, bool playMusic) : base(score, distance, distanceWeight, endDistance, gameInterval, playMusic)
        {
            this.EndDistance = 50;

            this.character.Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"STAGE 02\Lizard level.png"));
            this.character.Initialize();
            this.character.States = 2;
            this.character.ResizeWidth(120);
            this.character.Top = this.gameBox.Height - this.terrain.Height - this.character.Height;
            this.BaseHeight = this.character.Top;

            this.obstacles[1].Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"Obstacles\Grass.png"));
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
                this.Close();
                new Stage3(this.Score, 0, this.DistanceWeight, this.EndDistance, this.GameInterval - 4, this.PlayMusic).ShowDialog();
            }
        }
    }
}
