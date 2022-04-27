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

        public IActionResult OnGet(int id)
        {
            InfoText = "";

            FamilyGroupID = id;

            try
            {
                Members = MemberService.GetAllFamilyGroupMembers(id);
            }
            catch (SqlException sqlEx)
            {
                InfoText = "Database Error: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                InfoText = "General Error: " + ex.Message;
            }

            return Page();
        }
    }
}
