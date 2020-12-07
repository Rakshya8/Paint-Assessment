using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace Assessment1
{
    /// <summary>
    /// Class Polygon inherits base class Shape
    /// </summary>
    class Polygon : Shape
    {
        int[] points_list;
        /// <summary>
        /// Call base class constructor
        /// </summary>
        public Polygon() : base()
        {

        }
        /// <summary>
        /// Overloading Constructor and call overloaded base constructor
        /// </summary>
        /// <param name="color">Color Of Pen</param>
        /// <param name="fillshape">Inner Fill Shapes</param>
        /// <param name="x">X-axis Coordinate</param>
        /// <param name="y">Y-axis Coordinate</param>
        /// <param name="points">polygon points</param>
        public Polygon(Color color, bool fillshape, int x, int y, params int[] points) : base(color, fillshape, x, y)
        {
            points_list = points;
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
            this.points_list = list;            
        }

        /// <summary>
        /// New implementation of Draw method that is inherited from a base class.
        /// </summary>
        /// <param name="g">GDi+ Drawing surface</param>
        public override void Draw(Graphics g)
        {          
            Pen p = new Pen(c, 2);
            SolidBrush b = new SolidBrush(c);
            Point[] points = new Point[points_list.Length];
            int point_position = 0;
            for (int i = 0; i < points_list.Length; i += 2)
            {
                points[point_position] = new Point(points_list[i], points_list[i + 1]);
                point_position++;
            }
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
