using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace SecondSemesterProject.Models
{
    public class ShiftType
    {

        public int ShiftTypeId { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }


        public ShiftType(int shiftTypeId, string name, Color color)
        {
            ShiftTypeId = shiftTypeId;
            Name = name;
            Color = color;
        }

        public ShiftType()
        {
            
        }

        public override string ToString()
        {
            return $"Vagt type: {Name} Farve: {Color} Vagt type id: {ShiftTypeId}";
        }
    }
}
