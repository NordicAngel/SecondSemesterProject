using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Login
{
    public class LogoutModel : PageModel
    {
        private IMemberService MemberService;

        public Member Member { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public LogoutModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet()
        {
            if (MemberService.GetCurrentMember() != null)
            {
                Member = (Member)MemberService.GetCurrentMember();

                Email = Member.Email;
                Password = Member.Password;
            }
            else
            {
                return Redirect("~/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            MemberService.Logout();

            return Redirect("~/Index");
        }
    }
}
