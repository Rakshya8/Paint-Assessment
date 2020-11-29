using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    class rectangle : shape
    {
        int length;
        int width;


        public rectangle() : base()
        {

        }

        public rectangle(Color color, bool fillshape, int x, int y, int length, int width) : base(color, fillshape, x, y)
        {

            this.width = width;
            this.length = length;

        }

        public override void set(Color colour, bool fill, params int[] list)
        {
       
            base.set(colour, fill, list[0], list[1]);
            this.length = list[2];
            this.width = list[3];

        }

        public override void draw(Graphics g)
        {

            Pen p = new Pen(c, 2);
            SolidBrush b = new SolidBrush(c);
            if (fill)
            {
                g.FillRectangle(b, x, y, length, width);
            }
            else
            {
                g.DrawRectangle(p, x, y, length, width);
            }


        }
    }
}
