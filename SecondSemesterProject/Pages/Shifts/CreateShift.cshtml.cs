using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Shifts
{
    public class CreateShiftModel : PageModel
    {
        private IShiftService _shiftService;
        private IShiftTypeService _shiftTypeService;
        [BindProperty]
        public List<List<Shift>> Shifts { get; set; }
        public List<ShiftType> ShiftTypes { get; set; }

        public CreateShiftModel(IShiftService shiftService, IShiftTypeService shiftTypeService)
        {
            _shiftService = shiftService;
            _shiftTypeService = shiftTypeService;
        }

        public void OnGet()
        {
            ShiftTypes = _shiftTypeService.GetAllShiftTypesAsync();
            Shifts = new List<List<Shift>>()
            {
                new List<Shift>()
                {
                    new Shift(),
                    new Shift()
                },
                new List<Shift>()
                {
                    new Shift()
                }
            };
        }

        public void OnPostAddTimeSpan()
        {
            Shifts.Add(
                new List<Shift>()
                {
                    new Shift()
                }
            );
        }

        public void OnPostRemoveTimeSpan(int timeSpanIndex)
        {
            Shifts.RemoveAt(timeSpanIndex);
        }

        public void OnPostAddShift(int timeSpanIndex)
        {
            Shifts[timeSpanIndex].Add(
                    new Shift()
                );
        }

        public void OnPostRemoveShift(int timeSpanIndex, int shiftIndex)
        {
            Shifts[timeSpanIndex].RemoveAt(shiftIndex);
        }
    }
}
