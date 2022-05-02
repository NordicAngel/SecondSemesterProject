using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SecondSemesterProject.Services
{
    public class Connection
    {
        protected String ConnectionString;

        public Connection(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }


    }
}
