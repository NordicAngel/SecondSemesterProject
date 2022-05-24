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
        public int FamilyGroupID { get; set; }

        public string InfoText;

        public DeleteFamilyGroupModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet(int id)
        {
            FamilyGroupID = id;

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            InfoText = "";

            try
            {
                await MemberService.DeleteFamilyGroup(id);
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
