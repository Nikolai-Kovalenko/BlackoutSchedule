namespace BlackoutSchedule.Models.ViewModels
{
    public class HomeVM
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<Schedules> Schedules { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public bool IsLightNow()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            foreach (var schedule in Schedules)
            {
                if (currentTime >= schedule.StartTime && currentTime <= schedule.FinishTime)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
