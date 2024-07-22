using System.Data.Common;
using BlackoutSchedule.Data;
using BlackoutSchedule.Models;
using BlackoutSchedule.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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

        public void UnitUpdate(IEnumerable<Schedules> schedule, int groupId)
        {
            UpdateDeleteTimes(groupId);

            foreach (var i in schedule)
            {
                _db.Schedules.Add(i);
            }
        }

        private void UpdateDeleteTimes(int? groupId = null)
        {
            var currentTime = DateTime.Now;

            var schedulesQuery = _db.Schedules.AsQueryable();

            if (groupId != null)
            {
                schedulesQuery = schedulesQuery.Where(u => u.GroupId == groupId);
            }

            var schedulesList = schedulesQuery
                .Where(u => u.DeleteTime == null).ToList();

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
