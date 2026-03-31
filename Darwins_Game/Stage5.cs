using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sidescroller_Game
{
    internal class Stage5:GameWindowForm
    {
        public Stage5(int score, int distance, int distanceWeight, int endDistance, int gameInterval, bool playMusic) : base(score, distance, distanceWeight, endDistance, gameInterval, playMusic)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            this.EndDistance = 500;
            try
            {
                this.character.Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"STAGE 05\human stage.png"));
            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Found! \n {e.Message} \n {e.StackTrace}"); Console.ResetColor();
            }
            this.character.Initialize();
            this.character.States = 8;
            this.character.Flipper.Interval = 100;
            this.character.ResizeWidth(70);
     
            this.character.Top = this.gameBox.Height - this.terrain.Height - this.character.Height;
            this.BaseHeight = this.character.Top;

            this.obstacles[1].Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"Obstacles\Grass.png"));
            this.obstacles[1].Initialize();
            this.obstacles[1].ResizeWidth(100);
            this.obstacles[1].Top = 400;

            this.obstacles[2].Image = Image.FromFile(Utility.DirManipulation.PathToAsset(@"Obstacles\bomb.png"));
            this.obstacles[2].Initialize();
            this.obstacles[2].ResizeWidth(100);
            this.obstacles[2].Top = 400;

        }
        internal protected override void keyisProceed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.Distance >= this.EndDistance)
                {
                    Console.WriteLine(this.GameInterval);
                    this.Close();
                    new Stage1(this.Score, 0, this.DistanceWeight, this.EndDistance, this.GameInterval - 4, this.PlayMusic).ShowDialog();
                }
            }
        }
    }
}
