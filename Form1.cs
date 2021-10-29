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

            List<Point> newPoints = new List<Point>();

            newPoints.Add(points[0]);
            newPoints.Add(points[1]);
            newPoints.Add(points[2]);

            for(int i = 3; i < points.Count(); i++)
            {
                while(NotRightTurn(newPoints[newPoints.Count - 2], newPoints[newPoints.Count -1], points[i]))
                {
                    newPoints.RemoveAt(newPoints.Count() - 1);
                }
                newPoints.Add(points[i]);
            }

            LinkedList<Point> drpoints = new LinkedList<Point>();

            for (var i = 0; i < newPoints.Count(); i++)
                drpoints.AddLast(newPoints[i]);

            for (var iter = drpoints.First; iter != drpoints.Last; iter = iter.Next)
                g.DrawLine(myPen, iter.Value.x, iter.Value.y, iter.Next.Value.x, iter.Next.Value.y);
            g.DrawLine(myPen, drpoints.First.Value.x, drpoints.First.Value.y, drpoints.Last.Value.x, drpoints.Last.Value.y);
            pictureBox1.Image = bmp;
        }

        public bool NotRightTurn(Point a, Point b, Point c)
        {
            double res = (b.y - a.y) * (c.x - b.x) - (b.x - a.x) * (c.y - b.y);

            if (res >= 0)
                return true;
            else
                return false;
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
