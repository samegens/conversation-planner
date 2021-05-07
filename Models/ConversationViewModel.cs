using ConversationPlanner.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConversationPlanner.Models
{
    public class ConversationViewModel
    {
        public int Id { get; set; }

        [DisplayName("Participant 1")]
        [Required]
        public int Participant1Id { get; set; }

        [DisplayName("Participant 2")]
        [Required]
        public int Participant2Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public static ConversationViewModel FromConversation(Conversation conversation)
        {
            return new ConversationViewModel
            {
                Id = conversation.Id,
                Participant1Id = conversation.Participant1.Id,
                Participant2Id = conversation.Participant2.Id,
                Timestamp = conversation.Timestamp
            };
        }

        public Conversation ToConversation(ConversationPlannerContext context)
        {
            var participant1 = context.Participant.Single(p => p.Id == Participant1Id);
            var participant2 = context.Participant.Single(p => p.Id == Participant2Id);
            return new Conversation
            {
                Id = Id,
                Participant1 = participant1,
                Participant2 = participant2,
                Timestamp = Timestamp
            };
        }
    }
}
