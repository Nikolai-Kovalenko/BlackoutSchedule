using System.ComponentModel.DataAnnotations;

namespace BlackoutSchedule.Models.Dto
{
    public class GroupDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
