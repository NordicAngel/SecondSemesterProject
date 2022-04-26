using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Interfaces
{
    public interface IShiftService
    {
        void CreateShiftAsync(Shift shift);
        Shift GetShiftAsync(int shiftId);
        List<Shift> GetAllShiftAsync();
        void UpdateShiftAsync(int shiftId, Shift newShift);
        void DeleteShiftAsync(int shiftId);
    }
}
