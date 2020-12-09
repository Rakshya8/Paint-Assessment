using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment1
{
    class CustomExceptions :  Exception
    {
        public CustomExceptions()
        {

        }

        public CustomExceptions(string message) : base(message)
        {
            
        }
    }
}
