using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class VisitsRepository
    {
        public VisitsRepository()
        {
            using (var db = new VisitContext()) 
            { 
                db.Database.EnsureCreated();
                db.SaveChanges(); // dispose
            }   
        }

        public Visit GetById(string id)
        {
            using (var db = new VisitContext())
            {
                var visits = db.Visits;
                Visit visit = null;
                foreach (Visit v in visits)
                {
                    if (v.Record == id) visit = v;
                }
                return visit;
            }
        }

        public void Create(Visit entity)
        {
            using (var db = new VisitContext())
            {
                db.Visits.Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(Visit entity)
        {
            using (var db = new VisitContext())
            {
                db.Visits.Update(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Visit entity)
        {
            using (var db = new VisitContext())
            {
                db.Visits.Remove(entity);
                db.SaveChanges();
            }
        }

        public List<Visit> GetVisits()
        {
            using (var db = new VisitContext())
            {
                var query = from v in db.Visits
                            select v;
                return query.ToList<Visit>();
            }
        }

        /// <param name="type">False == for `Doctor`  or  True == for `Patient`</param>
        /// <param name="name">Name of necessary person</param>
        public List<Visit> GetVisits(bool type, string name)
        {
            using (var db = new VisitContext())
            {
                var query = (type) ?
                            from v in db.Visits
                            where v.Patient == name
                            select v
                            :
                            from v in db.Visits
                            where v.Doctor == name
                            select v;
                return query.ToList<Visit>();
            }
        }
    }
}
