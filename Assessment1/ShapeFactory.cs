using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment1
{
    class ShapeFactory
    {
        public shape getShape(String shapeType)
        {
            shapeType = shapeType.ToUpper().Trim(); //yoi could argue that you want a specific word string to create an object but I'm allowing any case combination


            if (shapeType.Equals("CIRCLE"))
            {
                return new circle();

            }
            else if (shapeType.Equals("RECTANGLE"))
            {
                return new rectangle();

            }
            else if (shapeType.Equals("TRAINGLE"))
            {
               return new traingle();
            }
            else
            {
                //if we get here then what has been passed in is inkown so throw an appropriate exception
                System.ArgumentException argEx = new System.ArgumentException("Factory error: " + shapeType + " does not exist");
                throw argEx;
            }


        }
    }
}
