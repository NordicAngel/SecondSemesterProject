using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Interfaces
{
    public interface IEventService
    {
        void CreateEventAsync(Event ev);
        Event GetEventAsync(int evId);
        List<Event> GetAllEventAsync();
        void UpdateEventAsync(int evId, Event ev);
        void DeleteEventAsync(int evId);
    }
}
