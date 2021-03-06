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

        public bool ResultsFound { get; set; }

        public string InfoText;

        public IndexModel(IMemberService service)
        {
            MemberService = service;

            Members = new List<IMember>();
        }

        public async Task<IActionResult> OnGet()
        {
            InfoText = "";

            try
            {
                if (!string.IsNullOrEmpty(FilterCriteria))
                {
                    Members = await MemberService.GetMembersByName(FilterCriteria);
                }
                else
                {
                    Members = await MemberService.GetAllMembers();
                }

                if (Members.Count > 0)
                {
                    ResultsFound = true;
                }
                else
                {
                    ResultsFound = false;
                }
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
