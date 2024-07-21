using BlackoutSchedule.Data;
using BlackoutSchedule.Models;
using BlackoutSchedule.Repository.IRepository;

namespace BlackoutSchedule.Repository
{
    public class GroupsReporitory : Repository<Groups>, IGroupsReporitory
    {
        private readonly AppDbContext _db;

        public GroupsReporitory(AppDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
