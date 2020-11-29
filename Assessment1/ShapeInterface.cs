using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    interface IShapes
    {
        void set(Color c, bool fillshape, params int[] list);
        void draw(Graphics g);

    }
}
