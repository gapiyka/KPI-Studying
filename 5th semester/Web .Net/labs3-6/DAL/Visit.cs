using System;

namespace DAL
{
    public class Visit
    {
        private string record;
        public string Record { get { return record; } }
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }

        public Visit(string Patient, string Doctor, DateTime Date, string Diagnosis)
        {
            this.Patient = Patient;
            this.Doctor = Doctor;
            this.Date = Date;
            this.Diagnosis = Diagnosis;
            this.record = this.Date.ToString() + "-" + this.Doctor;
        }
    }
}
