using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Profiles
{
    public class EditProfileModel : PageModel
    {
        private IMemberService MemberService;
        private IHostingEnvironment HostingEnvironment;

        [BindProperty]
        public Member Member { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        public EditProfileModel(IMemberService service, IHostingEnvironment environment)
        {
            MemberService = service;
            HostingEnvironment = environment;
        }

        public IActionResult OnGet(int id)
        {
            Member = (Member)MemberService.GetMemberByID(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string file = Path.Combine(HostingEnvironment.ContentRootPath, "wwwroot\\Images", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            //Member.ImageFileName = Upload.FileName;
            //MemberService.UpdateMember(Member.ID, Member);

            return RedirectToPage("/Profiles/Profile");
        }
    }
}
