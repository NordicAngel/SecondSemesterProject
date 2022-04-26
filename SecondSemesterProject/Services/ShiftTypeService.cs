using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class ShiftTypeService : IShiftTypeService
    {
        public void CreateShiftTypeAsync(ShiftType shiftType)
        {
            throw new NotImplementedException();
        }

        public ShiftType GetShiftTypeAsync(int shiftTypeId)
        {
            throw new NotImplementedException();
        }

        public List<ShiftType> GetAllShiftTypesAsync()
        {
            return new List<ShiftType>()
            {
                new ShiftType() {Color = Color.Blue, Name = "Bagger", ShiftTypeId = 1}
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
