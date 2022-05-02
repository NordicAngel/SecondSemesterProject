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
    public class IndexModel : PageModel
    {
        private IShiftService _shiftService;
        private IShiftTypeService _shiftTypeService;
        public List<IGrouping<DateTime, Shift>> DaysOfShifts { get; set; }

        [BindProperty] public DateTime FromDate { get; set; }
        [BindProperty] public int NumOfDays { get; set; }

        public IndexModel(IShiftService shiftService, IShiftTypeService shiftTypeService)
        {
            _shiftService = shiftService;
            _shiftTypeService = shiftTypeService;
        }

        public async Task OnGet()
        {
            FromDate = DateTime.Today;
            NumOfDays = 3;
            await SetPropsAsync();
        }

        public async Task OnPostReloadAsync()
        {
            await SetPropsAsync();
            NumOfDays = NumOfDays >= DaysOfShifts.Count 
                ? DaysOfShifts.Count - 1
                : NumOfDays;
        }

        private async Task SetPropsAsync()
        {
            DaysOfShifts = (await _shiftService.GetAllShiftAsync())
                .Where(s => s.DateTimeStart >= FromDate)
                .OrderBy(s => s.DateTimeStart)
                .GroupBy(s => s.DateTimeStart.Date)
                .ToList();
        }
    }
}
