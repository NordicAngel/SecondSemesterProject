using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
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

        public IMember Member { get; set; }

        public string InfoText;

        public LoginModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Member = MemberService.Login(Email, Password);

                if (Member != null)
                {
                    if (Member.BoardMember)
                    {
                        return RedirectToPage("/Members/Index");
                    }
                    else if (!Member.BoardMember)
                    {
                        return RedirectToPage("/Index");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                InfoText = "Database Error: " + sqlEx.Message;

                return Page();
            }
            catch (Exception ex)
            {
                InfoText = "General Error: " + ex.Message;

                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
