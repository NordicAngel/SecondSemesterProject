using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Pages.Login
{
    public class LogoutModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IMember Member { get; set; }

        public string InfoText;

        public LogoutModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
