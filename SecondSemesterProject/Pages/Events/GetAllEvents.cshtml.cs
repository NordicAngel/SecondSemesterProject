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
    public class GetAllEventsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        public List<Event> Events { get; private set; }
        public string ErrorMsg { get; set; }
        private IEventService eventService;

        public GetAllEventsModel(IEventService eService)
        {
            this.eventService = eService;
        }

        public async Task OnGetAsync()
        {
            if (!String.IsNullOrEmpty(FilterCriteria))
            {
                //Events = await eventService.GetEventsByNameAsync(FilterCriteria);
            }
            else
            {
                Events = await eventService.GetAllEventAsync();
            }
        }

        public async Task OnPostDelete(int EventId)
        {
            try
            {
                await eventService.DeleteEventAsync(EventId);
            }
            catch (DatabaseException ex)
            {
                ErrorMsg = ex.Message;
            }

            await OnGetAsync();
        }

        //public void OnGet()
        //{
        //}

        /*Kilde: 20220325_HotelDB21Starter - Pages, Hotels, GetAllHotels.cshtml
         , GetAllHotels.cshtml.cs*/

    }
}
