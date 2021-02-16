using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_10
{
    class EmptyExeption : ArgumentException
    {
        public EmptyExeption(string message)
            : base(message)
        { }
    }
    
}
