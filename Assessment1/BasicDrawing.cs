using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

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


        public BasicDrawing()
        {

        }

        public void SetBasicDrawing(string Draw, Color color, bool fillshape, params int[]  list )
        {
                int initX = list[0];
                int initY = list[1];

                string Drawing_command = Draw.Split('(')[0].Trim();               

                if (Drawing_command.Equals("circle"))
                {
                    int radius = int.Parse(Draw.Split('(', ')')[1]);
                    s = factory.getShape("circle");
                    s.Set(color, fillshape, initX, initY, radius);
                    shape_list.Add(s);

                }

                //Rectangle
                if (Drawing_command.Equals("rectangle"))
                {
                    string size = (Draw.Split('(', ')')[1]);
                    s = factory.getShape("rectangle");

                    int length = int.Parse(size.Split(',')[0]);
                    int width = int.Parse(size.Split(',')[1]);
                    s.Set(color, fillshape, initX, initY, length, width);
                    shape_list.Add(s);

                }
                //End Rectangle

                //Start Triangle
                if (Drawing_command.Equals("triangle"))
                {
                    string size = (Draw.Split('(', ')')[1]);
                    s = factory.getShape("triangle");

                    int side1 = int.Parse(size.Split(',')[0]);
                    int side2 = int.Parse(size.Split(',')[1]);
                    int side3 = int.Parse(size.Split(',')[2]);

                    s.Set(color, fillshape, initX, initY, side1, side2, side3);
                    shape_list.Add(s);

                }

                //End Triangle

                //Start Polygon
                if (Drawing_command.Equals("polygon"))
                {
                    string param = initX + "," + initY + "," + (Draw.Split('(', ')')[1]);
                    int[] points = Array.ConvertAll(param.Split(','), int.Parse);
                    s = factory.getShape("polygon");
                    s.Set(color, fillshape, points);
                    shape_list.Add(s);

                }
                //End Polygon
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList getShape()
        {
            return shape_list;
        }
    }
}
