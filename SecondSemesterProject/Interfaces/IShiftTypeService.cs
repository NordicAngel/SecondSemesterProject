using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Interfaces
{
    public interface IShiftTypeService
    {
        Task<bool> CreateShiftTypeAsync(ShiftType shiftType);
        Task<ShiftType> GetShiftTypeAsync(int shiftTypeId);
        Task<List<ShiftType>> GetAllShiftTypesAsync();
        void UpdateShiftTypeAsync(int shiftTypeId, ShiftType newShiftType);
        void DeleteShiftTypeAsync(int shiftTypeId);
    }
}
