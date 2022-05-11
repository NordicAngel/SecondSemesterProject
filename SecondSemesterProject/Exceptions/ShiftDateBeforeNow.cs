using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Exceptions
{
    public class ShiftDateBeforeNow : ShiftException
    {
        public ShiftDateBeforeNow(string message) : base(message)
        {

        }
    }
}
