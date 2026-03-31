using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Darwins_Game
{
    internal class Stage1 : GameWindowForm
    {
        public Stage1(int score = 0, int distance = 0, int distanceWeight = 1, int endDistance = 600, int gameInterval = 40, bool playMusic = true) : base(score, distance, distanceWeight, endDistance, gameInterval, playMusic)

        {
            this.EndDistance = 50;

        }
    }
}