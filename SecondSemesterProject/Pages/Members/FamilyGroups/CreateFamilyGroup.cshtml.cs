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
        public List<int?> MembersID { get; set; }

        public List<SelectListItem> Options { get; set; }

        public string InfoText;

        public CreateFamilyGroupModel(IMemberService service)
        {
            MemberService = service;

            MembersID = new List<int?>() {null, null, null, null, null};
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
                List<IMember> members = new List<IMember>();

                foreach (int? id in MembersID)
                {
                    if (id != null)
                    {
                        members.Add(MemberService.GetMemberByID((int)id));
                    }
                }

                MemberService.CreateFamilyGroup(members);
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

        public void CreateOptionsList()
        {
            Options = new List<SelectListItem>()
            {
                new SelectListItem("Ikke valgt", null)
            };

            Options.AddRange(MemberService.GetAllMembers().Select(a =>
                new SelectListItem
                {
                    Text = a.Name,
                    Value = a.ID.ToString()
                }).ToList());
        }
    }
}
