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

        [BindProperty]
        public Member Member { get; set; }

        public string InfoText;

        public ProfileModel(IMemberService service)
        {
            MemberService = service;

            Member = new Member();
        }

        public IActionResult OnGet()
        {
            InfoText = "";

            try
            {
                Member = (Member)MemberService.GetCurrentMember();
            }
            catch (NullReferenceException)
            {
                InfoText = "Ikke logget ind!";
            }

            return Page();
        }
    }
}
