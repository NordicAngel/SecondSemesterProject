using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;
using SecondSemesterProject.Services;

namespace SecondSemesterProject.Pages
{
    public  class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private IShiftService _shiftService;
        private IMemberService _memberService;

        [BindProperty]
        public List<Shift> MemShifts { get; set; }


        public IMemberService Mem { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger, IShiftService sService, IMemberService mService)
        {
            _logger = logger;
            _shiftService = sService;
            _memberService = mService;
        }


        public async Task<IActionResult>  OnGet()
        {
            if (_memberService.GetCurrentMember() != null)
            {
                int memberId = _memberService.GetCurrentMember().ID;
                MemShifts = (await _shiftService.GetShiftByMember(memberId))
                    .Where(s => s.DateTimeStart.Date >= DateTime.Today)
                    .ToList();
                return Page();
            }

           

            return Page();


        }
    }
}
