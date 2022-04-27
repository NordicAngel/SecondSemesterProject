using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class ShiftTypeService : ShiftType, IShiftTypeService
    {

        public List<ShiftType> Types { get; set; }

        public void CreateShiftTypeAsync(ShiftType shiftType)
        {
            
        }

        public ShiftType GetShiftTypeAsync(int shiftTypeId)
        {
            foreach (ShiftType type in Types)
            {
                if (type.ShiftTypeId == shiftTypeId)
                {
                    return type;
                }
            }

            return null;
        }

        public List<ShiftType> GetAllShiftTypesAsync()
        {
            return new List<ShiftType>()
            {
                new ShiftType() {Name = "Bager", Color = Color.Blue,  ShiftTypeId = 1}
            };
        }

        public void UpdateShiftTypeAsync(int shiftTypeId, ShiftType newShiftType)
        {
            throw new NotImplementedException();
        }

        public void DeleteShiftTypeAsync(int shiftTypeId)
        {
            throw new NotImplementedException();
        }

    }
}
