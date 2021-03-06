using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Exceptions;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Pages.Login
{
    public class LoginModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string InfoText;

        public LoginModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await MemberService.Login(Email, Password);

                if (MemberService.GetBoardMember())
                {
                    return RedirectToPage("/Profiles/Profile");
                }
            }
            catch (Exception ex)
            {
                InfoText = ex.Message;

                return Page();
            }

            return Redirect("~/Index");
        }
    }
}
