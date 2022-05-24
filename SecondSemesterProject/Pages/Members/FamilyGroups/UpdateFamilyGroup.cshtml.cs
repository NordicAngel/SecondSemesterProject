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

        public async Task<IActionResult> OnGet(int id)
        {
            FamilyGroupID = id;

            MembersID = MemberService.GetAllFamilyGroupMembers(id).Result.Select(a => (int?)a.ID).ToList();

            int count = MembersID.Count;

            for (int i = 0; i < 5 - count; i++)
            {
                MembersID.Add(null);
            }

            await CreateOptionsList();

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            InfoText = "";

            try
            {
                List<IMember> members = new List<IMember>();

                foreach (int? memberId in MembersID)
                {
                    if (memberId != null)
                    {
                        members.Add(await MemberService.GetMemberByID((int)memberId));
                    }
                }

                await MemberService.UpdateFamilyGroup(members, id);
            }
            catch (Exception ex)
            {
                InfoText = ex.Message;

                return Page();
            }

            return RedirectToPage("Index");
        }

        public async Task CreateOptionsList()
        {
            Options = new List<SelectListItem>()
            {
                new SelectListItem("Ikke valgt", null)
            };

            List<IMember> members = await MemberService.GetAllMembers();

            Options.AddRange(members.Select(a =>
                new SelectListItem
                {
                    Text = a.Name,
                    Value = a.ID.ToString()
                }).ToList());
        }
    }
}