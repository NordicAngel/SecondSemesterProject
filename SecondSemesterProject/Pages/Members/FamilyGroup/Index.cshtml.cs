using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Pages.Members.FamilyGroup
{
    public class IndexModel : PageModel
    {
        private IMemberService MemberService;

        public Dictionary<int, List<IMember>> FamilyGroups { get; set; }

        public string InfoText;

        public IndexModel(IMemberService service)
        {
            MemberService = service;

            FamilyGroups = new Dictionary<int, List<IMember>>();
        }

        public IActionResult OnGet()
        {
            InfoText = "";

            try
            {
                FamilyGroups = MemberService.GetAllFamilyGroups();
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