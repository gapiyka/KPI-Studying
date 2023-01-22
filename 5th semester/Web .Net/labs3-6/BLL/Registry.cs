using System;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class Registry
    {
        const string DB_ERROR = "Error with database. Comment: ";
        const string RES_EMPTY = "There is empty here.  .  .\n";
        const string DEFAULT_DIAGNOS = "Empty";
        public string patientName;
        public string doctorName;
        public VisitsRepository db;
        public Registry(string patient, string doctor)
        {
            this.patientName = patient;
            this.doctorName = doctor;
            this.db = new VisitsRepository();
        }

        public string Schedule()
        {
            string data = "";
            try
            {
                List<Visit> visitList = db.GetVisits(false, doctorName);
                foreach (Visit visit in visitList)
                {
                    DateTime date = visit.Date;
                    if (date.Ticks > DateTime.Now.Ticks)
                    {
                        data += ("|| DATE: " +  date.ToString() + " || DOCTOR: " + visit.Doctor + "\n ");
                    }
                } 
            } catch (Exception e)
            {
                return DB_ERROR + e.Message;
            }
            return (data != "") ? data : RES_EMPTY;
        }

        public string GetMedCard()
        {
            string data = "";
            try
            { 
                CheckADiagnosis();
                List<Visit> visitList = db.GetVisits(true, patientName);
                foreach (Visit visit in visitList)
                {
                    data += ("|| DATE: " + visit.Date.ToString() + " || DOCTOR: " + 
                        visit.Doctor + " || DIAGNOSIS: " + visit.Diagnosis + "\n ");
                }
            }
            catch (Exception e)
            {
                data = DB_ERROR + e.Message;
            }
            return (data != "") ? data : RES_EMPTY;
        }

        public string List()
        {
            string data = "";
            try
            {
                List<Visit> visitList = db.GetVisits();
                List<string> doctors = new List<string>();
                foreach (Visit visit in visitList)
                {
                    string doc = visit.Doctor;
                    if (!doctors.Contains(doc))
                    {
                        data += ("|| DOCTOR: " + doc + "\n ");
                        doctors.Add(doc);
                    }
                }
            }
            catch (Exception e)// **
            {
                data = DB_ERROR + e.Message;
            }
            return (data != "") ? data : RES_EMPTY;
        }

        public string SignUp(string date, string time)
        {
            string data = "";
            DateTime dateFormat;
            try
            {
                string[] dateArr = date.Split("/");
                string[] timeArr = time.Split(":");
                int[] dates = new int[] { Int32.Parse(dateArr[0]), Int32.Parse(dateArr[1]), Int32.Parse(dateArr[2]) };
                int[] times = new int[] { Int32.Parse(timeArr[0]), Int32.Parse(timeArr[1]) };
                if (times[0] < 8 || times[0] >= 18) return "WRONG: time, should be in range between 08:00 and 17:40\n";
                times[1] = (int)(times[1] / 20) * 20;
                dateFormat = new DateTime(dates[2], dates[0], dates[1], times[0], times[1], 0);
                if (dateFormat.Ticks < DateTime.Now.Ticks) return "WRONG: date, should be starts from today\n";
            }
            catch (Exception e)
            {
                return "WRONG: format of input. Check hints above.\n";// **
            }

            try
            {
                Visit exists = db.GetById(dateFormat.ToString() + "-" + doctorName);
                if (exists != null) return "WRONG: visit, at this time already exists.\n";
                Visit newVisit = new Visit(patientName, doctorName, dateFormat, DEFAULT_DIAGNOS);
                db.Create(newVisit);
                data = "We add your visit at " + dateFormat.ToString() + ". " + doctorName + " will wait for you.\n";
            }
            catch (Exception e)
            {
                return DB_ERROR + e.Message;
            }
            return (data != "") ? data : RES_EMPTY;
        }

        public string SetCurrentPatient(string newName)
        {
            patientName = newName;
            return "Your name changed at: " + newName + "\n";
        }

        public string SetCurrentDoctor(string newDoctor)
        {
            doctorName = newDoctor;
            return "Your doctor changed at: " + newDoctor + "\n";
        }

        public void CheckADiagnosis()
        {
            try
            {
                List<Visit> visitList = db.GetVisits(true, patientName);
                foreach (Visit visit in visitList)
                {
                    if (visit.Date.Ticks < DateTime.Now.Ticks && visit.Diagnosis == DEFAULT_DIAGNOS)
                    {
                        visit.Diagnosis = GenerateDiagnosis();
                        db.Update(visit);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(DB_ERROR + e.Message);
            }
        }

        public string GenerateDiagnosis()
        {
            const string DIAGNOSIS1 = "Healthy";
            const string DIAGNOSIS2 = "Light Cold";
            const string DIAGNOSIS3 = "Сoryza";
            const string DIAGNOSIS4 = "Astma";
            Random random = new Random(new DateTime().Millisecond);// **
            int res = random.Next(0, 100);
            if (res < 20) return DIAGNOSIS4;
            if (res < 40) return DIAGNOSIS3;
            if (res < 60) return DIAGNOSIS2;
            return DIAGNOSIS1;
        }
    }
}
