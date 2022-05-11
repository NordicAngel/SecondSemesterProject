using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Interfaces
{
    public interface IEventService
    {
        Task<bool> CreateEventAsync(Event ev);
        Task<Event> GetEventAsync(int evId);
        Task<List<Event>> GetAllEventAsync();
        Task UpdateEventAsync(int evId, Event ev);
        Task DeleteEventAsync(int evId);

        //Task<List<Event>> GetEventAsync(string filterCriteria);
    }
}
