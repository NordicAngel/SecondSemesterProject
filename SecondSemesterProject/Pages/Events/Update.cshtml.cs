using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;
using SecondSemesterProject.Services;

namespace SecondSemesterProject.Pages.Events
{
    public class UpdateModel : PageModel
    {
        private IEventService eventService;
        [BindProperty]
        public Event Event { get; set; }

        public UpdateModel(IEventService eventService)
        {
            this.eventService = eventService;
        }
        //public IActionResult OnGet(int evId)
        //{
        //    Event = eventService.GetEventAsync(evId).Result;
        //    return Page();
        //}

        public async Task<IActionResult> OnGetAsync(int evId)
        {
            Event = await eventService.GetEventAsync(evId);
            return Page();
        }

        public IActionResult OnPost(int evId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            eventService.UpdateEventAsync(evId,Event);
            return RedirectToPage("/Events/GetAllEvents");
        }
    }
}
