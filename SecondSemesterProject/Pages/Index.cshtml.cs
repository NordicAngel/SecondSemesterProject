using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages
{
    public  class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Shift> Shift { get; private set; }

        public IShiftService ShiftService;

        public IndexModel(ILogger<IndexModel> logger, IShiftService sService)
        {
            _logger = logger;
            ShiftService = sService;
        }

        


        public async Task OnGet()
        {
            

        }
    }
}
