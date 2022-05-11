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
    public class UpdateFamilyGroupModel : PageModel
    {
        private IMemberService MemberService;

        [BindProperty]
        public int FamilyGroupID { get; set; }

        [BindProperty]
        public List<int?> MembersID { get; set; }

        public List<SelectListItem> Options { get; set; }

        public string InfoText;

        public UpdateFamilyGroupModel(IMemberService service)
        {
            MemberService = service;

            MembersID = new List<int?>() { null, null, null, null, null };
        }

        public IActionResult OnGet(int id)
        {
            FamilyGroupID = id;

            MembersID = MemberService.GetAllFamilyGroupMembers(id).Select(a => (int?)a.ID).ToList();

            int count = MembersID.Count;

            for (int i = 0; i < 5 - count; i++)
            {
                MembersID.Add(null);
            }

            CreateOptionsList();

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            InfoText = "";

            try
            {
                List<IMember> members = new List<IMember>();

                foreach (int? memberId in MembersID)
                {
                    if (memberId != null)
                    {
                        members.Add(MemberService.GetMemberByID((int)memberId));
                    }
                }

                MemberService.UpdateFamilyGroup(members, id);
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