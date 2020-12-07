using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    /// <summary>
    /// Class Traingle inherits base class Shape
    /// </summary>
    class Triangle : Shape
    {
        int side1, side2, side3;

        /// <summary>
        /// Call base class constructor
        /// </summary>
        public Triangle() : base()
        {

        }

        /// <summary>
        /// Overloading Constructor and call overloaded base constructor
        /// </summary>
        /// <param name="color">Color Of Pen</param>
        /// <param name="fillshape">Inner Fill Shapes</param>
        /// <param name="x">X-axis Coordinate</param>
        /// <param name="y">Y-axis Coordinate</param>
        /// <param name="a">Side1 of Traingle</param>
        /// <param name="b">Side2 of Traingle</param>
        /// <param name="c">Side3 of Traingle</param>
        public Triangle(Color color,bool fillshape, int x, int y, int a, int b, int c) : base(color, fillshape, x, y)
        {

            this.side1 = a;
            this.side2 = b;
            this.side3 = c;
        }

        /// <summary>
        /// New implementation of Set method that is inherited from a base class.
        /// </summary>
        /// <param name="colour">Color of pen</param>
        /// <param name="fill">Inner fill shapes</param>
        /// <param name="list">stores number of arguments</param>
        public override void Set(Color colour,bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.Set(colour,fill, list[0], list[1]);
            this.side1 = list[2];
            this.side2 = list[3];
            this.side3 = list[4];

        }

        /// <summary>
        /// New implementation of Draw method that is inherited from a base class.
        /// </summary>
        /// <param name="g">GDi+ Drawing surface</param>
        public override void Draw(Graphics g)
        {

            Pen p = new Pen(c, 2);
            SolidBrush b = new SolidBrush(c);
            PointF[] points = new PointF[3];


            points[0].X = x;
            points[0].Y = y;

            points[1].X = x + side1;
            points[1].Y = y;

            points[2].X = x + side3;
            points[2].Y = y + side2;
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
