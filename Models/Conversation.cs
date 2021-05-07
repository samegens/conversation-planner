using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConversationPlanner.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Participant Participant1 { get; set; }

        [Required]
        public Participant Participant2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
