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
    public class ShiftModel : PageModel
    {
        private IShiftService _shiftService;
        private IShiftTypeService _shiftTypeService;
        private IMemberService _memberService;

        public ShiftModel(IShiftService shiftService, IShiftTypeService shiftTypeService, IMemberService memberService)
        {
            _shiftService = shiftService;
            _shiftTypeService = shiftTypeService;
            _memberService = memberService;
        }

        public Shift Shift { get; set; }
        public string MemberName { get; set; }
        public ShiftType ShiftType { get; set; }
        [BindProperty] public int DeleteOption { get; set; }
        [BindProperty] public DateTime DeleteUntilDate { get; set; }

        public async Task OnGetAsync(int id)
        {
            Shift = await _shiftService.GetShiftAsync(id);
            MemberName = Shift.MemberId == null? 
                "Ledig":
                "Taget af " + _memberService.GetMemberByID((int)Shift.MemberId).Name;
            ShiftType = await _shiftTypeService.GetShiftTypeAsync(Shift.ShiftTypeId);
        }

        public void OnPostTakeShift(int memberId)
        {

        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            Shift = await _shiftService.GetShiftAsync(id);
            switch (DeleteOption)
            {
                case 0:
                {
                    await _shiftService.DeleteShiftAsync(Shift.ShiftId);
                    break;
                }
                case 1:
                {
                    foreach (Shift s in (await FindSuccessorsInCategory(id)).Where(s => s.DateTimeStart.Date < DeleteUntilDate))
                    {
                        await _shiftService.DeleteShiftAsync(s.ShiftId);
                    }
                    break;
                }
                case 2:
                {
                    List<Shift> toRemove = await FindSuccessorsInCategory(id);
                    foreach (Shift s in toRemove)
                    {
                        await _shiftService.DeleteShiftAsync(s.ShiftId);
                    }

                    break;
                }
            }

            return RedirectToPage("/Shifts/Index");
        }

        private async Task<List<Shift>> FindSuccessorsInCategory(int id)
        {
            Shift = await _shiftService.GetShiftAsync(id);
            return (await _shiftService.GetAllShiftAsync())
                .Where(s => s.DateTimeStart >= Shift.DateTimeStart)
                .Where(s => s.DateTimeStart.TimeOfDay == Shift.DateTimeStart.TimeOfDay)
                .Where(s => s.DateTimeEnd.TimeOfDay == Shift.DateTimeEnd.TimeOfDay)
                .Where(s => s.ShiftTypeId == Shift.ShiftTypeId)
                .Where(s => (Shift.DateTimeStart - s.DateTimeStart).Days % 7 == 0)
                .ToList();
        }
    }
}
