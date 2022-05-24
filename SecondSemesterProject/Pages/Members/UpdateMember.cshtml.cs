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
    public class UpdateMemberModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public Member Member { get; set; }

        public string InfoText;

        public UpdateMemberModel(IMemberService service)
        {
            MemberService = service;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            Member = (Member) await MemberService.GetMemberByID(id);

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
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
                    await MemberService.UpdateMember(id, Member);
                }
                else
                {
                    InfoText = "E-mail eller telefonnummer allerede taget!";

                    return Page();
                }
            }
            catch (Exception ex)
            {
                InfoText = ex.Message;

                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
