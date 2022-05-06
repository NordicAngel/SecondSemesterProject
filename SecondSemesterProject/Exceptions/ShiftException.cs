using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Exceptions
{
    public class ShiftException : Exception
    {
        public ShiftException(string message):base(message){}
    }
}
