using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Interfaces
{
    public interface IShiftService
    {
        Task CreateShiftAsync(Shift shift);
        Task<Shift> GetShiftAsync(int shiftId);
        Task<List<Shift>> GetAllShiftAsync();
        Task UpdateShiftAsync(int shiftId, Shift newShift);
        Task DeleteShiftAsync(int shiftId);
    }
}
