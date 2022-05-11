using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SecondSemesterProject.Models
{
    public class Event
    {
        public int EventId { get; set; }
        //[Display(Name="Navn")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string Place { get; set; }
        public string Image { get; set; }

        public Event(int eventId, string name, string description, DateTime time, string place, string image)
        {
            EventId = eventId;
            Name = name;
            Description = description;
            Time = time;
            Place = place;
            Image = image;
        }

        public Event()
        {
            
        }

        public override string ToString()
        {
            return $"{nameof(EventId)}: {EventId}, {nameof(Name)}:{Name}" +
                   $"& {nameof(Description)}:{Description}";
        }
    }
}
