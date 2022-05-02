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
        Task<ShiftType> GetShiftTypeAsync(int id);
        Task<List<ShiftType>> GetAllShiftTypesAsync();
        Task<bool> UpdateShiftTypeAsync(int shiftTypeId, ShiftType newShiftType);
        Task<ShiftType> DeleteShiftTypeAsync(int shiftTypeId);
    }
}
