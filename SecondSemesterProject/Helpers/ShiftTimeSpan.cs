using System;
using System.Collections.Generic;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Helpers
{
    public class ShiftTimeSpan
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public List<Shift> TShifts { get; set; }

        public List<Shift> GetShifts()
        {
            TShifts.ForEach(s => s.DateTimeStart = DateTimeStart);
            TShifts.ForEach(s => s.DateTimeEnd = DateTimeEnd);
            return TShifts;
        }
    }
}