using System.ComponentModel.DataAnnotations;

namespace ConversationPlanner.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public string Tag { get; set; }

        [Required]
        public bool IsPresent { get; set; }
    }
}
