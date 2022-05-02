using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Pages.Members.FamilyGroup
{
    public class CreateFamilyGroupModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public List<IMember> Members { get; set; }

        public List<SelectListItem> Options { get; set; }

        public string InfoText;

        public CreateFamilyGroupModel(IMemberService service)
        {
            MemberService = service;

            Members = new List<IMember>();
        }

        public IActionResult OnGet()
        {
            CreateOptionsList();

            return Page();
        }

        public IActionResult OnPost()
        {
            InfoText = "";

            try
            {
                MemberService.CreateFamilyGroup(Members);
            }
            catch (SqlException sqlEx)
            {
                InfoText = "Database Error: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                InfoText = "General Error: " + ex.Message;
            }

            // Fix
            return Page();
        }

        public void CreateOptionsList()
        {
            Options = MemberService.GetAllMembers().Select(a =>
                new SelectListItem
                {
                    Value = a.ID.ToString(),
                    Text = a.Name
                }).ToList();
        }
    }
}
