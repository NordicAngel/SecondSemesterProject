using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Helpers
{
    public class ShiftGrouping : List<List<Shift>>, IGrouping<DateTime, List<Shift>>
    {
        private DateTime _key;

        public ShiftGrouping(DateTime key, IEnumerable<List<Shift>> collection) : base(collection) => _key = key;
      

        public DateTime Key => _key;
       
    }
}
