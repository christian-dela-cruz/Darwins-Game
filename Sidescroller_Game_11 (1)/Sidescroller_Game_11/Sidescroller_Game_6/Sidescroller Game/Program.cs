using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Sidescroller_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Leaderboards highScores = new Leaderboards();
            //highScores.PrintLeaderboard();
            Form menuForm = new Menu();
            menuForm.ShowDialog();
        }
    }
}
