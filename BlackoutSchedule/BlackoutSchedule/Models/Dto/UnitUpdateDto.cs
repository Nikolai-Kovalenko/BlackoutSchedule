using AutoMapper.Configuration.Conventions;

namespace BlackoutSchedule.Models.Dto
{
    public class UnitUpdateDto
    {
        public int groupId { get; set; }
        public string schedule { get; set; }
    }
}
