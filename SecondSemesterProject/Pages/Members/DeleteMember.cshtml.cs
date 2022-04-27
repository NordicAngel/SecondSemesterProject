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
    public class DeleteMemberModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public IMember Member { get; set; }

        public string InfoText;

        public DeleteMemberModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet(int id)
        {
            Member = MemberService.GetMemberByID(id);

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            InfoText = "";

            try
            {
                MemberService.DeleteMember(id);
            }
            catch (SqlException sqlEx)
            {
                InfoText = "Database Error\n" + sqlEx.Message;

                return Page();
            }
            catch (Exception ex)
            {
                InfoText = "General Error\n" + ex.Message;

                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
