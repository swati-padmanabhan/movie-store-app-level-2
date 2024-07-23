using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStoreLevel2.exceptions
{
    internal class MovieStoreIsEmptyException : Exception
    {
        public MovieStoreIsEmptyException(string message): base(message) { }

    }
}
