using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    class circle:shape
    {
        int radius;

        public circle() : base()
        {

        }

        public circle(Color color, bool fillshape, int x, int y, int radius) : base(color,fillshape, x, y)
        {
            this.radius = radius;
        }

        public override void set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(colour, fill, list[0], list[1]);
            this.radius = list[2];
        }

        public override void draw(Graphics g)
        {

            Pen p = new Pen(c, 2);
            SolidBrush b = new SolidBrush(c);
            if (fill)
            {
                g.FillEllipse(b, x, y, radius * 2, radius * 2);
            }
            else
            {
                g.DrawEllipse(p, x, y, radius * 2, radius * 2);
            }
            

        }
    }
}
