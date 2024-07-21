using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlackoutSchedule.Models.Dto
{
    public class AddressDto
    {
        [Key]
        public int Id { get; set; }

        public string Address { get; set; }

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Groups Group { get; set; }
    }
}
