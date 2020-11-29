using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    class traingle : shape
    {
        int side1, side2, side3;

        public traingle() : base()
        {

        }

        public traingle(Color color,bool fillshape, int x, int y, int a, int b, int c) : base(color, fillshape, x, y)
        {

            this.side1 = a;
            this.side2 = b;
            this.side3 = c;
        }

        public override void set(Color colour,bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(colour,fill, list[0], list[1]);
            this.side1 = list[2];
            this.side2 = list[3];
            this.side3 = list[4];

        }

        public override void draw(Graphics g)
        {

            Pen p = new Pen(c, 2);
            SolidBrush b = new SolidBrush(c);
            PointF[] points = new PointF[3];
            points[0].X = x;
            points[0].Y = y;

            points[1].X = x + side1;
            points[1].Y = y;

            points[2].X = x + side3;
            points[2].Y = y - side2;
            if (fill)
            {
                g.FillPolygon(b, points);
            }
            else
            {
                g.DrawPolygon(p, points);
            }


        }
    }
}
