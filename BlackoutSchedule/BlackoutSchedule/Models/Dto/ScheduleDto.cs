using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlackoutSchedule.Models.Dto
{
    public class ScheduleDto
    {

        [Key]
        public int Id { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Groups Group { get; set; }
    }
}
