using BlackoutSchedule.Models;
using static System.Collections.Specialized.BitVector32;

namespace BlackoutSchedule.Repository.IRepository
{
    public interface IScheduleRerository : IRepository<Schedules>
    {
        void UnitUpdate(Schedules schedule);

        void Update(List<Schedules> schedulesList);
    }
}
