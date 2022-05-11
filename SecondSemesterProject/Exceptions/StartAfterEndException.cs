using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Exceptions
{
    public class StartAfterEndException : ShiftException
    {
        public StartAfterEndException(string message) : base(message)
        {
            
        }
    }
}
