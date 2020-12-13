using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace Assessment1
{
    class BasicDrawing
    {
        //Object of class ShapeFactory
        ShapeFactory factory = new ShapeFactory();

        //Object of class Shape
        Shape s;

        //store list of objects.
        ArrayList shape_list = new ArrayList();

        //stores variables
        IDictionary<string, int> variable = new Dictionary<string, int>();

        /// <summary>
        /// default constructor
        /// </summary>
        public BasicDrawing()
        {

        }

        /// <summary>
        /// sets properties of shapes
        /// </summary>
        /// <param name="Draw">contains shape command</param>
        /// <param name="color">color of shape</param>
        /// <param name="fillshape">fill shape</param>
        /// <param name="list">contains x-axis, y-axis and shape size parameters</param>
        public void SetBasicDrawing(string Draw, Color color, bool fillshape, params int[] list)
        {
            variable = ComplexDrawing.getVariables();
            int initX = list[0];
            int initY = list[1];

            string Drawing_command = Draw.Split('(')[0];


            //circle (abc)
            //start circle command 
            if (Drawing_command.Equals("circle"))
            {
                string size = (Draw.Split('(', ')')[1]);
                int radius = 0;
                //checks if parameter is number.
                //if false then checks for variable used
                if (!Regex.IsMatch(size, @"^[0-9]+$"))
                {
                    variable.TryGetValue(size, out radius);
                }
                //else stores size passed directly
                else
                {
                    radius = int.Parse(size);
                }

                s = factory.getShape("circle");
                s.Set(color, fillshape, initX, initY, radius);
                shape_list.Add(s);
            }
            //end circle command

            // start Rectangle
            if (Drawing_command.Equals("rectangle"))
            {
                string size = (Draw.Split('(', ')')[1]);
                string[] param = size.Split(',');
                int height = 0;
                int width = 0;

                if (!Regex.IsMatch(param[0], @"^[0-9]+$"))
                {
                    variable.TryGetValue(param[0], out width);
                }
                else
                {
                    width = int.Parse(param[0]);
                }

                if (!Regex.IsMatch(param[1], @"^[0-9]+$"))
                {
                    variable.TryGetValue(param[1], out height);
                }
                else
                {
                    height = int.Parse(param[1]);
                }

                s = factory.getShape("rectangle");
                s.Set(color, fillshape, initX, initY, width, height);
                shape_list.Add(s);
            }
            //End Rectangle

            //Start Triangle
            if (Drawing_command.Equals("triangle"))
            {
                string size = (Draw.Split('(', ')')[1]);
                string[] param = size.Split(',');
                int side1 = 0;
                int side2 = 0;
                int side3 = 0;

                if (!Regex.IsMatch(param[0], @"^[0-9]+$"))
                {
                    variable.TryGetValue(param[0], out side1);
                }
                else
                {
                    side1 = int.Parse(param[0]);
                }
                if (!Regex.IsMatch(param[1], @"^[0-9]+$"))
                {
                    variable.TryGetValue(param[1], out side2);
                }
                else
                {
                    side2 = int.Parse(param[1]);
                }

                if (!Regex.IsMatch(param[2], @"^[0-9]+$"))
                {
                    variable.TryGetValue(param[2], out side3);
                }
                else
                {
                    side3 = int.Parse(param[2]);
                }
                s = factory.getShape("triangle");
                s.Set(color, fillshape, initX, initY, side1, side2, side3);
                shape_list.Add(s);

            }

            //End Triangle

            //Start Polygon
            if (Drawing_command.Equals("polygon"))
            {
                string param = initX + "," + initY + "," + (Draw.Split('(', ')')[1]);
                string[] p = param.Split(',');
                int[] points = new int[p.Length];
                for (int i = 0; i < p.Length; i++)
                {
                    if (!Regex.IsMatch(p[i], @"^[0-9]+$"))
                    {
                        variable.TryGetValue(p[i], out points[i]);
                    }
                    else
                    {
                        points[i] = int.Parse(p[i]);
                    }
                }
                s = factory.getShape("polygon");
                s.Set(color, fillshape, points);
                shape_list.Add(s);

            }
            //End Polygon
        }

        /// <summary>
        /// To return the shape commands stored in arraylist
        /// </summary>
        /// <returns>list of valid shape commands</returns>
        public ArrayList getShape()
        {
            return shape_list;
        }
    }
}
