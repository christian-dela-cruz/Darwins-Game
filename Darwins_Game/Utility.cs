using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Darwins_Game
{
    internal static class Utility
    {
        internal static class DirManipulation
        {
            public static string IntClimbDir(int steps = 1)
            {
                string path = Environment.CurrentDirectory;
                try
                {
                    do
                    {
                        path = Convert.ToString(Directory.GetParent(path));
                        steps--;
                    } while (steps > 0);
                }
                catch (DirectoryNotFoundException)
                {
                    return path;
                }
                return path;
            }
            public static string PathToAssets(string folderName = @"assets", int upsteps = 2)
            {
                string path = "";
                try
                {
                    if (folderName.Length == 0)
                    {
                        throw new ArgumentException();
                    }
                    path = IntClimbDir(upsteps);
                    path += string.Format(@"\" + folderName);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                return path;
            }
            public static string PathToAsset(string assetName)
            {
                string path;
                path = PathToAssets();
                path += String.Format(@"\"+assetName);
                return path;
            }
        }
        internal static class ImgManipulation
        {
            public static Size Object_SizeChanged(double ratio, int width, int height, int originalWidth, int originalHeight)
            {
                Size newSize = new Size(originalWidth, originalHeight);
                if (width != originalWidth)
                {
                    int newHeight = Convert.ToInt32(width / ratio);
                    newSize = new Size(width, newHeight);
                }
                else
                {
                    int newWidth = Convert.ToInt32(height * ratio);
                    newSize = new Size(newWidth, height);
                }
                originalWidth = width;
                originalHeight = height;
                return newSize;
            }
            public static Image SafeImportImage(string path)
            {
                Image image;
                try
                {
                    image = Image.FromFile(path);
                }
                catch (Exception e)
                {
                    image = null;
                }
                return image;
            }
            public static PictureBox HandleNullImage( PictureBox picbox, Color color = default(Color), Color textColor = default(Color))
            {
                if (color == default(Color)) { color = Color.White; textColor = Color.Red; }

                picbox.BackColor = color;
                Label label = new Label();
                label.ForeColor = textColor;
                label.Dock = DockStyle.Fill;
                label.TextAlign = ContentAlignment.TopCenter;
                label.Text = "ImageNotFound";
                picbox.Controls.Add(label);

                return picbox;
            }
        }
    }
}
