using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConversationPlanner.Models
{
    /// <summary>
    ///  ViewModel for showing a conversation in a list.
    /// </summary>
    public class ConversationListItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Participant 1")]
        public string Participant1 { get; set; }

        [DisplayName("Participant 2")]
        public string Participant2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime Timestamp { get; set; }

        public string CssStyle { get; set; }

        public static ConversationListItemViewModel FromConversation(Conversation conversation, bool isEvenRound)
        {
            return new ConversationListItemViewModel
            {
                Id = conversation.Id,
                Participant1 = conversation.Participant1.Name,
                Participant2 = conversation.Participant2.Name,
                Timestamp = conversation.Timestamp,
                CssStyle = isEvenRound ? "round-even" : "round-uneven"
            };
        }
    }
}
