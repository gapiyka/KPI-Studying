using System;

namespace SanitasLibr.Models
{
    public class MedCardModel : Output
    {
        public DateTime Date { get; set; }

        public string Doctor { get; set; }

        public string Diagnosis { get; set; }
    }
}
