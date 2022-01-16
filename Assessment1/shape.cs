using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    /// <summary>
    /// Abstract class Shape which declares the factory method
    /// </summary>
    abstract class Shape : IShapes
    {
        protected Color c, fc1, fc2;
        protected int x, y;
        protected bool fill;
        protected bool f;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Shape()
        {
            c = Color.Black;
            x = y = 0;
        }

        /// <summary>
        /// Overloading constructor with parameters
        /// </summary>
        /// <param name="colour">Color Of Pen</param>
        /// <param name="fillshape">Inner Fill Shapes</param>
        /// <param name="x">X-axis Coordinate</param>
        /// <param name="y">Y-axis Coordinate</param>
        public Shape(Color colour, bool fillshape, int x, int y)
        {

            this.c = colour;
            this.x = x;
            this.y = y;
            this.fill = fillshape;
        }

        /// <summary>
        /// Draw shape on panel
        /// </summary>
        /// <param name="g">GDI+ Drawing Surface</param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// Override set method derived from base class
        /// </summary>
        /// <param name="colour">Color of pen</param>
        /// <param name="fillshape">Inner fill shapes</param>
        /// <param name="list">stores number of arguments</param>
        public virtual void Set(Color colour, bool fillshape, params int[] list)
        {
            c = colour;
            fill = fillshape;
            x = list[0];
            y = list[1];
        }

        // <summary>
        /// Override set method derived from base class
        /// </summary>
        /// <param name="colour">Color of pen</param>
        /// <param name="fillshape">Inner fill shapes</param>
        /// <param name="list">stores number of arguments</param>
        public virtual void Set(Color colour, bool fillshape, bool flash, Color c1, Color c2, params int[] list)
        {
            c = colour;
            fill = fillshape;
            x = list[0];
            y = list[1];
            f = flash;
            fc1 = c1;
            fc2 = c2;

        }


    }
}
