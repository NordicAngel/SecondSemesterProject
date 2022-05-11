using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Profiles
{
    public class ProfileModel : PageModel
    {
        private IMemberService MemberService;
        private IShiftTypeService ShiftTypeService;

        [BindProperty]
        public Member Member { get; set; }

        [BindProperty]
        public Dictionary<int, bool> ShiftTypes { get; set; }

        public ProfileModel(IMemberService service, IShiftTypeService shiftTypeService)
        {
            MemberService = service;
            ShiftTypeService = shiftTypeService;

            ShiftTypes = new Dictionary<int, bool>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (MemberService.CheckCurrentMember())
            {
                Member = (Member)MemberService.GetCurrentMember();

                foreach (int id in await MemberService.GetMemberShiftTypes(Member.ID))
                {
                    ShiftTypes.Add(id, true);
                }

                foreach (ShiftType shiftType in await ShiftTypeService.GetAllShiftTypesAsync())
                {
                    if (!CheckShiftType(shiftType.ShiftTypeId))
                    {
                        ShiftTypes.Add(shiftType.ShiftTypeId, false);
                    }
                }
            }
            else
            {
                return Redirect("~/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Member = (Member)MemberService.GetCurrentMember();

            await MemberService.UpdateMemberShiftTypes(Member.ID, ShiftTypes);

            return Page();
        }

        private bool CheckShiftType(int id)
        {
            foreach (KeyValuePair<int, bool> shiftType in ShiftTypes)
            {
                if (shiftType.Key == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}