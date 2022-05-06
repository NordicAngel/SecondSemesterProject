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

        public Dictionary<ShiftType, bool> ShiftTypes { get; set; }

        public ProfileModel(IMemberService service, IShiftTypeService shiftTypeService)
        {
            MemberService = service;
            ShiftTypeService = shiftTypeService;

            ShiftTypes = new Dictionary<ShiftType, bool>();
        }

        public async Task<IActionResult> OnGet()
        {
            if (MemberService.CheckCurrentMember())
            {
                Member = (Member)MemberService.GetCurrentMember();

                foreach (KeyValuePair<int, bool> item in await MemberService.GetMemberShiftTypes(Member.ID))
                {
                    ShiftTypes.Add(await ShiftTypeService.GetShiftTypeAsync(item.Key), item.Value);
                }
            }
            else
            {
                return Redirect("~/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Dictionary<int, bool> shiftTypes = new Dictionary<int, bool>();

            foreach (KeyValuePair<ShiftType, bool> item in ShiftTypes)
            {
                shiftTypes.Add(item.Key.ShiftTypeId, item.Value);
            }

            await MemberService.UpdateMemberShiftTypes(Member.ID, shiftTypes);

            return Page();
        }
    }
}