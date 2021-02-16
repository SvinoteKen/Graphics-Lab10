using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_10
{
    class OutOfRange:ArgumentException
{
    public OutOfRange(string message)
        : base(message)
    { }
}
}
