using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Pages.Admins
{
    public class IndexModel : PageModel
    {
        public IMemberService MemberService { get; set; }

        public IndexModel(IMemberService service)
        {
            MemberService = service;
        }

        public IActionResult OnGet()
        {
            if (MemberService.GetCurrentMember().BoardMember)
            {
                return Page();
            }

            return Redirect("~/Index");
        }
    }
}
