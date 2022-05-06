using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class IndexModel : PageModel
    {
        private IShiftService _shiftService;
        private IShiftTypeService _shiftTypeService;
        public List<ShiftGrouping> DaysOfShifts { get; set; }

        [BindProperty] 
        public DateTime FromDate { get; set; }
        [BindProperty] public int NumOfDays { get; set; }
        public string ErrMsg { get; set; }

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
            if (ErrMsg != null)
                return;
            NumOfDays = NumOfDays >= DaysOfShifts.Count 
                ? DaysOfShifts.Count 
                : NumOfDays;
        }

        public async Task OnPostNextAsync()
        {
            await SetPropsAsync();

            if (ErrMsg != null)
                return;

            NumOfDays = NumOfDays >= DaysOfShifts.Count 
                ? DaysOfShifts.Count 
                : NumOfDays;

            if (DaysOfShifts.Count != 0)
            {
                DateTime time = DaysOfShifts[NumOfDays-1].Key + new TimeSpan(1, 0, 0, 0);
                if (time < DaysOfShifts.Max(x=>x.Key))
                {
                    FromDate = time;
                    await OnPostReloadAsync();
                }
            }
        }

        private async Task SetPropsAsync()
        {
            try
            {
                var list0 = (await _shiftService.GetAllShiftAsync())
                        .Where(s => s.DateTimeStart >= FromDate)
                        .OrderBy(s => s.DateTimeStart)
                        .GroupBy(s => s.DateTimeStart.Date)
                        .ToList();

            //Days
            var list1 = new List<ShiftGrouping>();
            foreach (var a in list0)
            {
                //day of shift coloums
                var list2 = new List<List<Shift>>();
                    
                //coloum of shifts
                foreach (var shift in a)
                {
                    for (int i = 0; true; i++)
                    {
                        if (list2.Count < i + 1)
                        {
                            list2.Add(new List<Shift>());
                        }

                        if (list2[i].Count == 0 || list2[i][^1].DateTimeEnd <= shift.DateTimeStart)
                        {
                            list2[i].Add(shift);
                            break;
                        }
                    }
                }
                list1.Add(new ShiftGrouping(a.Key,list2));
            }
            DaysOfShifts = list1;
            }
            catch (DatabaseException dbEx)
            {
                ErrMsg = dbEx.Message;
            }
        }
    }
}
