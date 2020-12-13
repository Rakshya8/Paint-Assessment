using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Assessment1
{
    /// <summary>
    /// Class Circle inherits base class Shape
    /// </summary>
    class Circle : Shape
    {
        int radius;

        /// <summary>
        /// Call base class constructor
        /// </summary>
        public Circle() : base()
        {

        }
        /// <summary>
        /// Overloading Constructor and call overloaded base constructor
        /// </summary>
        /// <param name="color">Color Of Pen</param>
        /// <param name="fillshape">Inner Fill Shapes</param>
        /// <param name="x">X-axis Coordinate</param>
        /// <param name="y">Y-axis Coordinate</param>
        /// <param name="radius">Radius Of Circle</param>
        public Circle(Color color, bool fillshape, int x, int y, int radius) : base(color, fillshape, x, y)
        {
            this.radius = radius;
        }

        /// <summary>
        /// New implementation of Set method that is inherited from a base class.
        /// </summary>
        /// <param name="colour">Color of pen</param>
        /// <param name="fill">Inner fill shapes</param>
        /// <param name="list">stores number of arguments</param>
        public override void Set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.Set(colour, fill, list[0], list[1]);
            this.radius = list[2];
        }

        /// <summary>
        /// New implementation of Draw method that is inherited from a base class.
        /// </summary>
        /// <param name="g">GDi+ Drawing surface</param>
        public override void Draw(Graphics g)
        {
            if (Form1.RotateShape() != 0)
            {
                float rotateValue = (float)Form1.RotateShape();
                g.RotateTransform((rotateValue));
            }
            Pen p = new Pen(c, 2);
            SolidBrush b = new SolidBrush(c);
            if (fill)
            {
                g.FillEllipse(b, x, y, radius, radius);
            }
            else
            {
                g.DrawEllipse(p, x, y, radius, radius);
            }


        }
    }
}
