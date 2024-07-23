using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStoreLevel2.exceptions
{
    internal class CapacityIsFullException : Exception
    {
        public CapacityIsFullException(string message):base(message) { }

    }
}
