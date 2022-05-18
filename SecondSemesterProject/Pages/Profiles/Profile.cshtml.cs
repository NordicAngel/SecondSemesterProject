using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Profiles
{
    public class ProfileModel : PageModel
    {
        private IMemberService MemberService;
        private IShiftTypeService ShiftTypeService;
        private IHostingEnvironment HostingEnvironment;

        [BindProperty]
        public Member Member { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public Dictionary<int, bool> ShiftTypes { get; set; }

        public ProfileModel(IMemberService service, IShiftTypeService shiftTypeService, IHostingEnvironment hostingEnvironment)
        {
            MemberService = service;
            ShiftTypeService = shiftTypeService;
            HostingEnvironment = hostingEnvironment;

            ShiftTypes = new Dictionary<int, bool>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (MemberService.CheckCurrentMember())
            {
                Member = (Member)MemberService.GetCurrentMember();

                foreach (int id in await MemberService.GetMemberShiftTypes(Member.ID))
                {
                    ShiftTypes.Add(id, true);
                }

                foreach (ShiftType shiftType in await ShiftTypeService.GetAllShiftTypesAsync())
                {
                    if (!CheckShiftType(shiftType.ShiftTypeId))
                    {
                        ShiftTypes.Add(shiftType.ShiftTypeId, false);
                    }
                }
            }
            else
            {
                return Redirect("~/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Upload != null)
            {
                await UploadImage();
            }

            await MemberService.UpdateMember(Member.ID, Member);

            await MemberService.UpdateMemberShiftTypes(Member.ID, ShiftTypes);

            await MemberService.UpdateCurrentMember(Member.ID);

            return Page();
        }

        private bool CheckShiftType(int id)
        {
            foreach (KeyValuePair<int, bool> shiftType in ShiftTypes)
            {
                if (shiftType.Key == id)
                {
                    return true;
                }
            }

            return false;
        }

        private async Task UploadImage()
        {
            string file = Path.Combine(HostingEnvironment.ContentRootPath, "wwwroot\\Images", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            Member.ImageFileName = Upload.FileName;
        }
    }
}