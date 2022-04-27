using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Models
{
    public class Event
    {
        int EventId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        DateTime Time { get; set; }
        string Place { get; set; }
        string Image { get; set; }

    }
}
