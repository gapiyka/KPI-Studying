using System;

namespace SanitasLibr.Models
{
    public class ScheduleModel : Output
    {
        public DateTime Date { get; set; }

        public string Doctor { get; set; }
    }
}
