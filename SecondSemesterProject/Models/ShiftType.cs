using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace SecondSemesterProject.Models
{
    public class ShiftType
    {
        
        public string Name { get; set; }
        public Color Color { get; set; }
        public int ShiftTypeId { get; set; }

        public ShiftType(string name, Color color, int shiftTypeId)
        {
            Name = name;
            Color = color;
            ShiftTypeId = shiftTypeId;
        }
    }
}
