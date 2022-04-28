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

        public IActionResult OnGet(int id)
        {
            Member = (Member)MemberService.GetMemberByID(id);

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            InfoText = "";

            try
            {
                MemberService.UpdateMember(id, Member);
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
