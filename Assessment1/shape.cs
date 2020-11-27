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

        public shape()
        {
            c = Color.Black;
            x = y = 0;
        }
        public shape(Color colour, int x, int y)
        {

            this.c = colour; 
            this.x = x; 
            this.y = y; 
        }

        public abstract void draw(Graphics g, bool fill);

        public virtual void set(Color colour, params int[] list)
        {
            this.c = colour;
            this.x = list[0];
            this.y = list[1];
        }


    }
}
