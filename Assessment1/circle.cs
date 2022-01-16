using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Assessment1
{
    /// <summary>
    /// Class Circle inherits base class Shape
    /// </summary>
    class Circle : Shape
    {
        int radius;
        Thread newThread;
        bool running = true, check = false;

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
        public override void Set(Color colour, bool fill, bool flash, Color c1, Color c2, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.Set(colour, fill, flash, c1, c2, list[0], list[1]);
            this.radius = list[2];
            f = flash;
            fc1 = c1;
            fc2 = c2;
            Console.WriteLine(f);
        }

        /// <summary>
        /// New implementation of Draw method that is inherited from a base class.
        /// </summary>
        /// <param name="g">GDi+ Drawing surface</param>
        public override void Draw(Graphics g)
        {
            Console.WriteLine(f);
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
            if (f == true)
            {
                //create the newthread passing the delegate method thread() which corresponds to the ThreadStart delegate (void method())
                newThread = new System.Threading.Thread(delegate ()
                {
                    while (true) //don't allow (in this case) for it to terminate
                    {
                        while (running == true)
                        {
                            if (check == false)
                            {
                                p = new Pen(fc1);
                                b = new SolidBrush(fc1);
                                if (fill)
                                {
                                    g.FillEllipse(b, x, y, radius, radius);
                                }
                                else
                                {
                                    g.DrawEllipse(p, x, y, radius, radius);
                                }

                                check = true;

                            }
                            else
                            {
                                p = new Pen(fc2);
                                b = new SolidBrush(fc2);
                                if (fill)
                                {
                                    g.FillEllipse(b, x, y, radius, radius);
                                }
                                else
                                {
                                    g.DrawEllipse(p, x, y, radius, radius);
                                }

                                check = false;

                            }
                            Thread.Sleep(1000);
                        }
                    }
                });
                newThread.Start(); //make the thread execute
                if (f == false)
                {
                    newThread.Abort();
                }


            }
        }
    }
}
