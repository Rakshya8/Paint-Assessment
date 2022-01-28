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
    /// Class Rectangle inherits base class Shape
    /// </summary>
    class Rectangle : Shape
    {
        int length;
        int width;
        Thread newThread;
        bool running = true, check = false;

        /// <summary>
        /// Call base class constructor
        /// </summary>
        public Rectangle() : base()
        {

        }
        /// <summary>
        /// Overloading Constructor and call overloaded base constructor
        /// </summary>
        /// <param name="color">Color Of Pen</param>
        /// <param name="fillshape">Inner Fill Shapes</param>
        /// <param name="x">X-axis Coordinate</param>
        /// <param name="y">Y-axis Coordinate</param>
        /// <param name="length">Length of Rectangle</param>
        /// <param name="width">Width of Rectangle</param>
        public Rectangle(Color color, bool fillshape, int x, int y, int length, int width) : base(color, fillshape, x, y)
        {

            this.width = width;
            this.length = length;

        }

        /// <summary>
        /// New implementation of Set method that is inherited from a base class.
        /// </summary>
        /// <param name="color">Color of pen</param>
        /// <param name="fill">Inner fill shapes</param>
        /// <param name="list">stores number of arguments</param>
        public override void Set(Color color, bool fill, bool flash, Color c1, Color c2, params int[] list)
        {

            base.Set(color, fill, list[0], list[1]);
            this.length = list[2];
            this.width = list[3];
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
            if (Form1.RotateShape() != 0)
            {
                float rotateValue = (float)Form1.RotateShape();
                g.RotateTransform((rotateValue), MatrixOrder.Append);
            }
            if (Form1.TranslateX() != 0 && Form1.TranslateY() != 0)
            {
                float translateX = (float)Form1.TranslateX();
                float translateY = (float)Form1.TranslateY();
                g.TranslateTransform(translateX, translateY);
            }
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
                                Pen p2 = new Pen(fc1);
                                SolidBrush b2 = new SolidBrush(fc1);
                                if (fill)
                                {
                                    g.FillRectangle(b2, x, y, length, width);
                                }
                                else
                                {
                                    g.DrawRectangle(p2, x, y, length, width);
                                }

                                check = true;

                            }
                            else
                            {
                                Pen p1 = new Pen(fc2);
                                SolidBrush b1 = new SolidBrush(fc2);
                                if (fill)
                                {
                                    g.FillRectangle(b1, x, y, length, width);
                                }
                                else
                                {
                                    g.DrawRectangle(p1, x, y, length, width);
                                }

                                check = false;

                            }
                            Thread.Sleep(1000);
                        }
                    }
                });
                newThread.Start(); //make the thread execute               


            }


        }
    }
}
