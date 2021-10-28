using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graham
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static Bitmap bmp = new Bitmap(561, 426);

        Graphics g = Graphics.FromImage(bmp);

        Pen myPen = new Pen(Color.Black);

        public class Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        List<Point> points = new List<Point>();
        Random rand = new Random();
        Random rand_n = new Random();

        void RandomGeneratePoints()
        {
            
            int n = rand_n.Next(1, 100);
            while(n != 0)
            {
                points.Add(new Point(rand.Next(0, pictureBox1.Width - 1), rand.Next(0, pictureBox1.Height - 1)));
                n--;
            }
        }

        void DrawRandomPoints()
        {
            RandomGeneratePoints();
            foreach(var iter in points)
            {
                bmp.SetPixel(iter.x, iter.y, Color.Black);
            }
            pictureBox1.Image = bmp;
        }

        //static void Swap<T>(ref T lhs, ref T rhs)
        //{
        //    T temp;
        //    temp = lhs;
        //    lhs = rhs;
        //    rhs = temp;
        //}

        void Graham()
        {
            Point minpoint = new Point(pictureBox1.Width, pictureBox1.Height);
            int num = 0;
            for (var iter = 0; iter < points.Count; iter++)
            {
                if (points[iter].y < minpoint.y || (points[iter].y == minpoint.y && points[iter].x < minpoint.x))
                {
                    minpoint.y = points[iter].y;
                    minpoint.x = points[iter].x;
                    num = iter;
                }
            }
            var x = points[0];
            points[0] = points[num];
            points[num] = points[0];

            points.Sort((a, b) => GetPolarAngle(a.y - points[0].y, a.x - points[0].x).CompareTo(GetPolarAngle(b.y - points[0].y, b.x - points[0].x)));

            Point p1 = points[0];
            Point p2 = points.Last();

            List<Point> up_side = new List<Point>();
            List<Point> down_side = new List<Point>();

            up_side.Add(p1);
            down_side.Add(p1);
            for(var i = 1; i < points.Count(); ++i)
            {
                if (i == points.Count() - 1 || Up(p1, points[i], p2))
                {
                    while (up_side.Count() >= 2 && !Up(up_side[up_side.Count() - 2], up_side[up_side.Count() - 1], points[i]))
                        up_side.RemoveAt(up_side.Count() - 1);
                    up_side.Add(points[i]);
                }
                if (i == points.Count() - 1 || Down(p1, points[i], p2))
                {
                    while (down_side.Count() >= 2 && !Down(down_side[down_side.Count() - 2], down_side[down_side.Count() - 1], points[i]))
                        down_side.RemoveAt(down_side.Count() - 1);
                    down_side.Add(points[i]);
                }
            }
            points.Clear();
            for (var i = 0; i < up_side.Count(); ++i)
                points.Add(up_side[i]);
            for (var i = 0; i < down_side.Count(); ++i)
                points.Add(down_side[i]);

            LinkedList<Point> drpoints = new LinkedList<Point>();

            for (var i = 0; i < points.Count(); i++)
                drpoints.AddLast(points[i]);

            for (var iter = drpoints.First; iter != drpoints.Last; iter = iter.Next)
                g.DrawLine(myPen, iter.Value.x, iter.Value.y, iter.Next.Value.x, iter.Next.Value.y);
            pictureBox1.Image = bmp;
        }

        bool Up(Point a, Point b, Point c)
        {
            return a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y) < 0;
        }

        bool Down(Point a, Point b, Point c)
        {
            return a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y) > 0;
        }

        double GetPolarAngle(int y, int x)
        {
            double res = Math.Atan2(y, x);

            if (res < 0) res += 2 * Math.PI;

            return res;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            DrawRandomPoints();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            points = new List<Point>();
            g.Clear(Color.White);
            pictureBox1.Image = bmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graham();
        }
    }
}
