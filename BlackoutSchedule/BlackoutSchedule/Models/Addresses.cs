using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackoutSchedule.Models
{
    public class Addresses
    {

        [Key]
        public int Id { get; set; }

        public string Address { get; set; }

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Groups Group { get; set; }
    }
}
