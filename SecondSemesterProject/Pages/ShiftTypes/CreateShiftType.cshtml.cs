using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.ShiftTypes
{
    public class CreateShiftTypeModel : PageModel
    {
        public IShiftTypeService ShiftTypeService;

        [BindProperty]
        public ShiftType shiftType { get; set; }


        public CreateShiftTypeModel(IShiftTypeService catalog)
        {
            ShiftTypeService = catalog;
        }



        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await ShiftTypeService.CreateShiftTypeAsync(shiftType);
            return RedirectToPage("GetAllShiftType");
        }
    }
}
