using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Pages.Members
{
    public class FamilyGroupModel : PageModel
    {
        private IMemberService MemberService;

        public int FamilyGroupID { get; set; }

        public List<IMember> Members { get; set; }

        public string InfoText;

        public FamilyGroupModel(IMemberService service)
        {
            MemberService = service;

            Members = new List<IMember>();
        }

        public async Task<IActionResult> OnGet(int id)
        {
            InfoText = "";

            FamilyGroupID = id;

            try
            {
                Members = await MemberService.GetAllFamilyGroupMembers(id);
            }
            catch (Exception ex)
            {
                InfoText = ex.Message;

                return Page();
            }

            return Page();
        }
    }
}
