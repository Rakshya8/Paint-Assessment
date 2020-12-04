using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assessment1
{
    /// <summary>
    /// The Shape interface
    /// </summary>
    interface IShapes
    {
        /// <summary>
        /// Set properties of shape
        /// </summary>
        /// <param name="colour">Color of pen</param>
        /// <param name="fill">Inner fill shapes</param>
        /// <param name="list">stores number of arguments</param>
        void Set(Color c, bool fillshape, params int[] list);
        
        /// <summary>
        /// Draw shape in panel
        /// </summary>
        /// <param name="g">GDI+ Drawing Surface</param>
        void Draw(Graphics g);

    }
}
