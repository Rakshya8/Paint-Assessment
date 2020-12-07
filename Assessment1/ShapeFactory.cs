using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment1
{
    /// <summary>
/// Class ShapeFactory controls the type of shape object to create
/// </summary>
     class ShapeFactory
    {
        /// <summary>
        /// check user requirement for shape and return that shape
        /// </summary>
        /// <param name="shapeType">Shape of object</param>
        /// <returns>type of shape required</returns>
        public Shape getShape(String shapeType)
        {
            shapeType = shapeType.ToUpper().Trim();


            if (shapeType.Equals("CIRCLE"))
            {
                return new Circle();

            }
            else if (shapeType.Equals("RECTANGLE"))
            {
                return new Rectangle();

            }
            else if (shapeType.Equals("TRIANGLE"))
            {
               return new Triangle();
            }
            else if (shapeType.Equals("POLYGON"))
            {
                return new Polygon();
            }
            else
            {
                
                System.ArgumentException argEx = new System.ArgumentException("Factory error: " + shapeType + " does not exist");
                throw argEx;
            }


        }
    }
}
