using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.ShiftTypes
{
    public class DeleteShiftTypeModel : PageModel
    {
        public IShiftTypeService ShiftTypeSevice;

        [BindProperty]
        public ShiftType shiftType { get; set; }

        public DeleteShiftTypeModel(IShiftTypeService catalog)
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
            await ShiftTypeSevice.DeleteShiftTypeAsync(shiftTypeId);
            return RedirectToPage("GetAllShiftType");
        }
    }
}
