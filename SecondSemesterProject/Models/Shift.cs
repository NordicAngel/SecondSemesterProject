using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Models
{
    public class Shift
    {
        public int ShiftId { get; set; }
        public DateTime DateTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public int MemberId { get; set; }
    }
}
