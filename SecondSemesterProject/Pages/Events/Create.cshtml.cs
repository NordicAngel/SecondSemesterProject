using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Exceptions;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Pages.Events
{
    public class CreateModel : PageModel
    {
        private IEventService eventService;
        [BindProperty]
        public Event Event { get; set; }
        public string ErrorMsg { get; set; }

        public CreateModel(IEventService eventService)
        {
            this.eventService = eventService;
        }
        
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await eventService.CreateEventAsync(Event);
            }
            catch (DatabaseException ex)
            {
                ErrorMsg = ex.Message;
                return Page();
            }

            return RedirectToPage("/Events/GetAllEvents");
        }
    }
}
