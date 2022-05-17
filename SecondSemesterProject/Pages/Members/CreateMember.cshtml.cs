using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Members
{
    public class CreateMemberModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public Member Member { get; set; }

        public string InfoText;

        public CreateMemberModel(IMemberService service)
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

            InfoText = "";

            try
            {
                if (await MemberService.CheckMemberInfo(Member))
                {
                    await MemberService.CreateMember(Member);
                }
                else
                {
                    InfoText = "E-mail eller telefonnummer allerede taget!";

                    return Page();
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