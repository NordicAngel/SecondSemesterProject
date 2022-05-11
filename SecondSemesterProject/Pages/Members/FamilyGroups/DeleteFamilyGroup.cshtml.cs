using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Members.FamilyGroups
{
    public class DeleteFamilyGroupModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public int FamilyGroupId { get; set; }

        public string InfoText;

        public DeleteFamilyGroupModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet(int id)
        {
            FamilyGroupId = id;

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            InfoText = "";

            try
            {
                MemberService.DeleteFamilyGroup(id);
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
