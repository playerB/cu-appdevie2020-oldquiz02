using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quiz2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random rd = new Random();
        double gameTime = 0;
        int[,] grassBugTable = new int[8, 8];
        int mode = 2; //1 = grass, 2 = bug
        bool isFull = false;

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int dx = e.X / (pictureBox1.Width / 8);
            int dy = e.Y / (pictureBox1.Height / 8);
            if (mode == grassBugTable[dx,dy])
            {
                grassBugTable[dx, dy] = 0;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0 ; i <= 8 ; i++)
            {
                Point v1 = new Point(pictureBox1.Width * i / 8, 0);
                Point v2 = new Point(pictureBox1.Width * i / 8, pictureBox1.Height);
                Point h1 = new Point(0, pictureBox1.Height * i / 8);
                Point h2 = new Point(pictureBox1.Width, pictureBox1.Height * i / 8);
                g.DrawLine(Pens.Black, v1, v2);
                g.DrawLine(Pens.Black, h1, h2);
            }
            for (int tx = 0 ; tx < 8 ; tx++)
                for (int ty = 0; ty < 8; ty++)
                {
                    if (grassBugTable[tx, ty] == 1)
                    {
                        g.DrawString("grass", DefaultFont, Brushes.Green, pictureBox1.Width * tx / 8, pictureBox1.Height * ty / 8);
                    }else if (grassBugTable[tx, ty] == 2)
                    {
                        g.DrawString("bug", DefaultFont, Brushes.Yellow, pictureBox1.Width * tx / 8, pictureBox1.Height * ty / 8);
                    }
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isFull == true)
            {
                timer1.Stop();
                MessageBox.Show("ENDED");
                
            }
            else gameTime += 1;
            label2.Text = "time : " + (gameTime / 10) + " s";
            isFull = true;
            foreach (int i in grassBugTable)
            {
                if (i == 0) isFull = false;
            }
            if (gameTime % 5 == 0)
            {
                int grassOrBug = rd.Next(1, 3); //1 = grass, 2 = bug
                bool filled = false;
                do
                {
                    int tableX = rd.Next(0, 8);
                    int tableY = rd.Next(0, 8);
                    if (grassBugTable[tableX, tableY] == 0)
                    {
                        grassBugTable[tableX, tableY] = grassOrBug;
                        filled = true;
                    }
                } while (filled == false);
            }
            Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                mode = 2;
                label1.Text = "ยาฆ่าแมลง";
                button3.Text = "เปลี่ยนเป็นยาฆ่าหญ้า";
            }
            else if (mode == 2)
            {
                mode = 1;
                label1.Text = "ยาฆ่าหญ้า";
                button3.Text = "เปลี่ยนเป็นยาฆ่าแมลง";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
            for (int tx = 0; tx < 8; tx++)
                for (int ty = 0; ty < 8; ty++)
                {
                    sw.WriteLine("{0},{1},{2}", tx, ty, grassBugTable[tx, ty]);
                }
            sw.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            StreamReader sr = new StreamReader(openFileDialog1.FileName);
            grassBugTable.Initialize();
            String line = "";
            String[] splitline;
            while ((line = sr.ReadLine()) != null)
            {
                splitline = line.Split(',');
                int tx = Convert.ToInt32(splitline[0]);
                int ty = Convert.ToInt32(splitline[1]);
                int v = Convert.ToInt32(splitline[2]);
                grassBugTable[tx, ty] = v;
            }
            sr.Close();
            timer1.Start();
        }
    }
}
