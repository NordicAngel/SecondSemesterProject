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
    public class UpdateShiftTypeModel : PageModel
    {
        public IShiftTypeService ShiftTypeSevice;

        [BindProperty]
        public ShiftType shiftType { get; set; }

        [BindProperty]
        public Color Color { get; set; }


        public UpdateShiftTypeModel(IShiftTypeService catalog)
        {
            ShiftTypeSevice = catalog;

        }

        public async Task<IActionResult> OnGet(int shiftTypeId)
        {
            shiftType = await ShiftTypeSevice.GetShiftTypeAsync(shiftTypeId);
            return Page();
        }

        public async Task<IActionResult> OnPost(int shiftTypeId)
        {
            await ShiftTypeSevice.UpdateShiftTypeAsync(shiftTypeId, shiftType);
            return RedirectToPage("GetAllShiftType");
        }
    }
}
