using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Interfaces
{
    public interface IShiftTypeService
    {
        void CreateShiftTypeAsync(ShiftType shiftType);
        ShiftType GetShiftTypeAsync(int shiftTypeId);
        List<ShiftType> GetAllShiftTypesAsync();
        void UpdateShiftTypeAsync(int shiftTypeId, ShiftType newShiftType);
        void DeleteShiftTypeAsync(int shiftTypeId);
    }
}
