using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
   abstract class shape : IShapes
    {
        protected Color c;
        protected int x, y;
        protected bool fill;

        public shape()
        {
            c = Color.Black;
            x = y = 0;
        }
        public shape(Color colour, bool fillshape, int x, int y)
        {

            this.c = colour; 
            this.x = x; 
            this.y = y;
            this.fill = fillshape;
        }

        public abstract void draw(Graphics g);

        public virtual void set(Color colour,bool fillshape, params int[] list)
        {
            this.c = colour;
            this.fill = fillshape;
            this.x = list[0];
            this.y = list[1];
            
                 
        }


    }
}
