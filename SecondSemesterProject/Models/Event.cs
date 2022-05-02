using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Models
{
    public class Event
    {
        public int EventId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        DateTime Time { get; set; }
        string Place { get; set; }
        string Image { get; set; }

        public Event(int eventId, string name, string description, DateTime time, string place, string image)
        {
            EventId = eventId;
            Name = name;
            Description = description;
            //Time = time;
            Place = place;
            Image = image;
        }

        public override string ToString()
        {
            return $"{nameof(EventId)}: {EventId}, {nameof(Name)}:{Name}" +
                   $"& {nameof(Description)}:{Description}";
        }
    }
}
