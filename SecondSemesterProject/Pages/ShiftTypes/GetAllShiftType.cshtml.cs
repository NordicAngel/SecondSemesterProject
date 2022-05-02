using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.ShiftTypes
{
    public class GetAllShiftTypeModel : PageModel
    {
        public List<ShiftType> ShiftTypes { get; private set; }

        private IShiftTypeService shiftTypeService;

        public GetAllShiftTypeModel(IShiftTypeService sService)
        {
            shiftTypeService = sService;
        }

        public async Task OnGet()
        {

            ShiftTypes = await shiftTypeService.GetAllShiftTypesAsync();
            
            
        }

    }
}
