using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
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
        private IEventService _eventService;
        private IParticipantService _participantService;
        private IMemberService _memberService;

        public GetAllEventsModel(IEventService eventService, IParticipantService participantService, IMemberService memberService)
        {
            _eventService = eventService;
            _participantService = participantService;
            _memberService = memberService;
        }

        public async Task OnGetAsync()
        {
            //if (!String.IsNullOrEmpty(FilterCriteria))
            //{
            //    Events = await _eventService.GetEventAsync(FilterCriteria);
            //}
            //else
            {
                Events = await _eventService.GetAllEventAsync();
            }
        }

        public async Task OnPostDelete(int eventId)
        {
            try
            {
                await _eventService.DeleteEventAsync(eventId);
            }
            catch (DatabaseException ex)
            {
                ErrorMsg = ex.Message;
            }

            await OnGetAsync();
        }

        public async Task<IActionResult> OnPostParticipate(int eventId)
        {
            if (_memberService.GetCurrentMember() == null)
                return RedirectToPage("/Events/GetAllEvents");

            int memId = _memberService.GetCurrentMember().ID;
            await _participantService.CreateParticipantAsync(memId, eventId);
            return RedirectToPage("/Events/GetAllEvents");
        }

        public async Task<IActionResult> OnPostCancelParticipation(int eventId)
        {
            if (_memberService.GetCurrentMember() == null)
                return RedirectToPage("/Events/GetAllEvents");

            int memId = _memberService.GetCurrentMember().ID;
            await _participantService.DeleteParticipantAsync(memId, eventId);
            return RedirectToPage("/Events/GetAllEvents");
        }

        //public void OnGet()
        //{
        //}

        /*Kilde: 20220325_HotelDB21Starter - Pages, Hotels, GetAllHotels.cshtml
         , GetAllHotels.cshtml.cs*/

    }
}
