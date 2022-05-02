using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Models
{
    public class Shift : ICloneable
    {
        public int ShiftId { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public int ShiftTypeId { get; set; }
        public int? MemberId { get; set; }
        public object Clone()
        {
            return new Shift()
            {
                ShiftId = ShiftId,
                DateTimeStart = DateTimeStart,
                DateTimeEnd = DateTimeEnd,
                ShiftTypeId = ShiftTypeId,
                MemberId = MemberId
            };
        }
    }
}
