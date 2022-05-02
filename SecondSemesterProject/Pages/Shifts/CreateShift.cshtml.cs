using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Helpers;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Shifts
{
    public partial class CreateShiftModel : PageModel
    {
        private IShiftService _shiftService;
        private IShiftTypeService _shiftTypeService;
        [BindProperty]
        public List<ShiftTimeSpan> TimeSpans { get; set; }
        [BindProperty] 
        public DateTime Date { get; set; }
        [BindProperty]
        public string RepeatUntilWeek { get; set; }
        [BindProperty] 
        public bool RepeatWeekly { get; set; }
        public List<ShiftType> ShiftTypes { get; set; }

        public CreateShiftModel(IShiftService shiftService, IShiftTypeService shiftTypeService)
        {
            _shiftService = shiftService;
            _shiftTypeService = shiftTypeService;
            ShiftTypes = _shiftTypeService.GetAllShiftTypesAsync();
        }

        public void OnGet()
        {
            ShiftTypes = _shiftTypeService.GetAllShiftTypesAsync();
            Shifts = new List<List<Shift>>()
            {
                new()
                {
                    TShifts = new List<Shift>()
                    {
                        new ()
                    }
                }
            };
        }

        public async Task OnPostAsync()
        {
            List<Shift> allShifts = new List<Shift>();
            foreach (ShiftTimeSpan timeSpan in TimeSpans)
            {
                timeSpan.GetShifts().ForEach(s => allShifts.Add(s));
            }
            allShifts.ForEach(s => s.DateTimeStart = Date.Date + s.DateTimeStart.TimeOfDay);
            allShifts.ForEach(s => s.DateTimeEnd = Date.Date + s.DateTimeEnd.TimeOfDay);

            //Adds All the weekly repeats
            if (RepeatWeekly)
            {
                DateTime repeatFrom = Date + new TimeSpan(7,0,0,0);
                int year = Convert.ToInt32(RepeatUntilWeek.Split('-')[0]);
                int week = Convert.ToInt32(RepeatUntilWeek.Split('W')[1]);
                DateTime toDate = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

                List<Shift> shiftsToDupe = allShifts.ConvertAll(s => (Shift)s.Clone()).ToList();

                while (repeatFrom < toDate)
                {
                    shiftsToDupe.ForEach(s=> s.DateTimeStart = repeatFrom.Date + s.DateTimeStart.TimeOfDay);
                    shiftsToDupe.ForEach(s=> s.DateTimeEnd = repeatFrom.Date + s.DateTimeEnd.TimeOfDay);

                    List<Shift> shiftDupes = shiftsToDupe.ConvertAll(s => (Shift)s.Clone()).ToList();

                    allShifts.AddRange(shiftDupes);
                    repeatFrom = repeatFrom.AddDays(7);
                }
            }

            foreach (Shift shift in allShifts)
            {
                await _shiftService.CreateShiftAsync(shift);
            }
        }

        public void OnPostAddTimeSpan()
        {
            TimeSpans.Add(
                new ShiftTimeSpan()
                {
                    TShifts = new List<Shift>()
                    {
                        new ()
                    }
                }
            );
        }

        public void OnPostRemoveTimeSpan(int timeSpanIndex)
        {
            TimeSpans.RemoveAt(timeSpanIndex);
        }

        public void OnPostAddShift(int timeSpanIndex)
        {
            TimeSpans[timeSpanIndex].TShifts.Add(
                    new Shift()
                );
        }

        public void OnPostRemoveShift(int timeSpanIndex, int shiftIndex)
        {
            TimeSpans[timeSpanIndex].TShifts.RemoveAt(shiftIndex);
        }
    }
}
