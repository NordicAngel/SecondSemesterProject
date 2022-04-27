using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SecondSemesterProject.Services
{
    public abstract class Connection
    {
        protected string connectionString = @"";

        public Connection(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}