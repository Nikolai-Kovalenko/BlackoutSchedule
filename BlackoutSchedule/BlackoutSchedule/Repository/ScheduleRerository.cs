using System.Data.Common;
using BlackoutSchedule.Data;
using BlackoutSchedule.Models;
using BlackoutSchedule.Repository.IRepository;
using static System.Collections.Specialized.BitVector32;

namespace BlackoutSchedule.Repository
{
    public class ScheduleRerository : Repository<Schedules>, IScheduleRerository
    {
        private readonly AppDbContext _db;

        public ScheduleRerository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void UnitUpdate(Schedules schedule)
        {
            DateTime deleteTime = DateTime.Now;
            var schedulesList = _db.Schedules.Where(u => u.GroupId == schedule.GroupId && u.DeleteTime == null).ToList();
            if (schedulesList != null)
            {
                foreach (var i in schedulesList)
                {
                    i.DeleteTime = deleteTime;
                }
            }
        }

        private void UpdateDeleteTimes()
        {
            var currentTime = DateTime.Now;

            List<Schedules> schedulesList = _db.Schedules.Where(u => u.DeleteTime == null).ToList();

            foreach (var i in schedulesList)
            {
                i.DeleteTime = currentTime;
            }

            _db.SaveChanges();
        }

        public void Update(List<Schedules> schedulesList)
        {
            var currentTime = DateTime.Now;

            UpdateDeleteTimes();

            foreach (var i in schedulesList)
            {
                _db.Schedules.Add(i);
            }
        }
    }
}
