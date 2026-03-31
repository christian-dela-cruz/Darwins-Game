using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sidescroller_Game
{
    internal class Leaderboards
    {
        private Score[] leaderboard = new Score[10];
        private string path = Utility.DirManipulation.PathToAssets("saved data");
        private string fileName = "leaderboards.txt";
        protected internal class Score
        {
            private string name;
            private int points;
            public Score(string name, int score)
            {
                this.name = name;
                this.points = score;
            }
            protected internal string Name
            {
                get { return name; }
                set { name = value; }
            }
            protected internal int Points
            {
                get { return points; }
                set { points = value; }
            }
        }
        public Leaderboards()
        {
            for (int i = 0; i < this.leaderboard.Length; i++)
            {
                leaderboard[i] = new Score(String.Empty, 0);
            }
            this.UpdateLeaderboard();
        }
        internal int UpdateLeaderboard() // reads data from leaderboard.txt and updates the local leaderboard
        {
            string dataString = string.Empty;
            string[] dataStringArray;
            bool validPath = false;
            string path = this.path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Directory created: {0}", path);
            }
            path += string.Format(@"\" + this.fileName);
            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException();
                }
                else
                {
                    validPath = true;
                }
            }
            catch (FileNotFoundException)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Close();
                for (int i = 0; i < this.leaderboard.Length; i++)
                {
                    leaderboard[i] = new Score(String.Empty, 0);
                }
            }
            if (validPath)
            {
                StreamReader sr = new StreamReader(path);
                string entry = sr.ReadLine();
                while (entry != null)
                {
                    dataString += entry;
                    dataString += ";";
                    try
                    {
                        entry = sr.ReadLine();
                    }
                    catch (System.IO.EndOfStreamException)
                    {
                        break;
                    }
                }
                dataString = dataString.TrimEnd(';');
                sr.Close();
            }
            dataStringArray = dataString.Split(';');
            for (int i = 0; i < dataStringArray.Length; i++)
            {
                string[] data = dataStringArray[i].Split('|');
                try
                {
                    RecordScore(ref this.leaderboard, data[0], Convert.ToInt32(data[1]));
                }
                catch (FormatException)
                {
                    continue;
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
            return dataStringArray.Length;
        }
        protected internal void RecordScore(ref Score[] lb, string name, int points, int index = 0) // inserts a score into the local sorted leaderboard
        {
            bool appended = false;
            byte totalData = (byte)lb.Length;
            for (int i = 0; i < totalData - 1; i++)
            {
                if (points <= lb[i].Points)
                { continue; }
                else
                {
                    for (int j = totalData - 2; j >= i; j--)
                    {
                        lb[j + 1] = lb[j];
                    }
                    lb[i] = new Score(name, points);
                    appended = true;
                    break;
                }
            }
            if (!appended && points > lb[totalData-1].Points)
            {
                lb[totalData-1] = new Score(name, points);
            }
        }
         internal static void WriteData(Score score) // this overload will append to the leaderboards.txt file
        {
            Leaderboards lb = new Leaderboards();
            string path = lb.path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Directory created: {0}", path);
            }
            path += string.Format(@"\" + lb.fileName);
            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Close();
            }
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(String.Format(score.Name + '|' + score.Points));
            }
        }
        protected internal void WriteData(ref Score[] lb) // this overload will overwrite the leaderboard.txt with only the top scores recorded locally
        {
            string path = this.path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Directory created: {0}", path);
            }
            path += string.Format(@"\" + this.fileName);
            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Close();
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < lb.Length / 2; i++)
                {
                    if (lb[i].Name.Length > 0)
                    {
                        sw.Write(lb[i].Name);
                        sw.Write('|');
                        sw.WriteLine(lb[i].Points);
                    }
                }
            }
        }
        protected internal void PrintLeaderboard(bool all = false)
        {
            sbyte sentinel = 0;
            if (all)
            {
                sentinel = -1;
            }
            for (int i = 0; i < this.leaderboard.Length; i++)
            {
                if (this.leaderboard[i].Name.Length > sentinel)
                {
                    Console.Write(i + 1);
                    Console.WriteLine(". ===============================\n");
                    Console.WriteLine("\tName:");
                    Console.WriteLine("\t\t{0}", this.leaderboard[i].Name);
                    Console.WriteLine("\tScore:");
                    Console.WriteLine("\t\t{0}\n", this.leaderboard[i].Points);
                }
            }
        }
        protected internal string GetName(int index)
        {
            return this.leaderboard[index].Name;
        }
        protected internal int GetScore(int index)
        {
            return this.leaderboard[index].Points;
        }
        protected internal int TotalRows
        { get { return this.leaderboard.Length; } }
    }
}
