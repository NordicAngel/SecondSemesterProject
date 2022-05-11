using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Exceptions;
using SecondSemesterProject.Helpers;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Shifts
{
    public class CreateShiftModel : PageModel
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
        public string ErrMsg { get; set; }

        public CreateShiftModel(IShiftService shiftService, IShiftTypeService shiftTypeService)
        {
            try
            {
                _shiftService = shiftService;
                _shiftTypeService = shiftTypeService;
                ShiftTypes = _shiftTypeService.GetAllShiftTypesAsync().Result;
            }
            catch (Exception Ex)
            {
                ErrMsg = Ex.Message;
            }
        }

        public void OnGetAsync()
        {
            Date = DateTime.Today;
            TimeSpans = new ()
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

        public async Task<IActionResult> OnPostAsync()
        {
            #region validations

            if (Date < DateTime.Today)
            {
                ModelState.AddModelError("Date","Dato må ikke være før i dag");
            }

            for(int i = 0; i < TimeSpans.Count; i++)
            {
                if (TimeSpans[i].DateTimeEnd <= TimeSpans[i].DateTimeStart)
                {
                    ModelState.AddModelError($"TimeSpans[{i}].DateTimeEnd","Slut tid skal være efter start tid");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            #endregion
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

            try
            {
                foreach (Shift shift in allShifts)
                {
                    await _shiftService.CreateShiftAsync(shift);
                }
            }
            catch (DatabaseException dbEx)
            {
                ErrMsg = dbEx.Message;
            }

            return RedirectToPage("/Shifts/index");
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
