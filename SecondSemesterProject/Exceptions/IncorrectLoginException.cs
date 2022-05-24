using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Exceptions
{
    public class IncorrectLoginException : Exception
    {
        public IncorrectLoginException(string message) : base(message)
        {
            
        }
    }
}