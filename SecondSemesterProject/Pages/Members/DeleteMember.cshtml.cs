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
    public class DeleteMemberModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public Member Member { get; set; }

        public string InfoText;

        public DeleteMemberModel(IMemberService service)
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
            InfoText = "";

            try
            {
                await MemberService.DeleteMember(id);
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
