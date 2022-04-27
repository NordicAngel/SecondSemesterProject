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
    public class IndexModel : PageModel
    {
        private IMemberService MemberService;

        public List<IMember> Members { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        public string InfoText;

        public IndexModel(IMemberService service)
        {
            MemberService = service;

            Members = new List<IMember>();
        }

        public IActionResult OnGet()
        {
            InfoText = "";

            try
            {
                if (!string.IsNullOrEmpty(FilterCriteria))
                {
                    Members = MemberService.GetMembersByName(FilterCriteria);
                }
                else
                {
                    Members = MemberService.GetAllMembers();
                }
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
