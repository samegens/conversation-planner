using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConversationPlanner.Models;

namespace ConversationPlanner.Data
{
    public class ConversationPlannerContext : DbContext
    {
        public ConversationPlannerContext (DbContextOptions<ConversationPlannerContext> options)
            : base(options)
        {
        }

        public void Seed()
        {
            if (!Participant.Any())
            {
                var bart = AddParticipant(1, "Bart");
                var daan = AddParticipant(2, "Daan");
                var glen = AddParticipant(3, "Glen");
                var peter = AddParticipant(4, "Peter");
                var sebastiaan = AddParticipant(5, "Sebastiaan");
                var yuksel = AddParticipant(6, "Yuksel");
                var willem = AddParticipant(7, "Willem", "RSO");
                var tuba = AddParticipant(8, "Tuba", "M&O");
                var ineke = AddParticipant(9, "Ineke", "ComIT");
                var wouter = AddParticipant(10, "Wouter", "DIA");
                var jasper = AddParticipant(11, "Jasper", "DIA");
                var sophie = AddParticipant(12, "Sophie", "O&S");
                var richard = AddParticipant(13, "Richard", "TSO");
                var egbert = AddParticipant(14, "Egbert", "LTSO");
                var nick = AddParticipant(15, "Nick", "DIA");
                var rens = AddParticipant(16, "Rens", "DIA");
                var rik = AddParticipant(17, "Rik", "DIA");
                var walter = AddParticipant(18, "Walter", "DIA");
                var jeroen = AddParticipant(19, "Jeroen", "SRE");
                var sandra = AddParticipant(20, "Sandra", "ComIT");
                var robin = AddParticipant(21, "Robin", "RIO");

                var round1 = new DateTime(2021, 5, 4, 15, 40, 0);
                AddConversation(willem, tuba, round1);
                AddConversation(ineke, wouter, round1);
                AddConversation(jasper, sophie, round1);
                AddConversation(richard, egbert, round1);
                AddConversation(nick, rens, round1);
                AddConversation(rik, walter, round1);
                AddConversation(jeroen, sandra, round1);
                AddConversation(glen, robin, round1);

                var round2 = new DateTime(2021, 5, 4, 15, 50, 0);
                AddConversation(willem, robin, round2);
                AddConversation(ineke, tuba, round2);
                AddConversation(jasper, wouter, round2);
                AddConversation(richard, sophie, round2);
                AddConversation(nick, egbert, round2);
                AddConversation(rik, rens, round2);
                AddConversation(jeroen, walter, round2);
                AddConversation(daan, sandra, round2);

                var round3 = new DateTime(2021, 5, 4, 16, 0, 0);
                AddConversation(willem, sandra, round3);
                AddConversation(ineke, robin, round3);
                AddConversation(jasper, tuba, round3);
                AddConversation(richard, wouter, round3);
                AddConversation(nick, sophie, round3);
                AddConversation(rik, egbert, round3);
                AddConversation(jeroen, rens, round3);
                AddConversation(peter, walter, round3);
            };
        }

        private void AddConversation(Participant participant1, Participant participant2, DateTime timestamp)
        {
            var conversation = new Conversation
            {
                Participant1 = participant1,
                Participant2 = participant2,
                Timestamp = timestamp
            };
            Add(conversation);
        }

        private Participant AddParticipant(int id, string name, string tag = null)
        {
            var participant = new Participant
            {
                Id = id,
                Name = name,
                Tag = tag
            };
            Add(participant);
            return participant;
        }

        public DbSet<ConversationPlanner.Models.Participant> Participant { get; set; }

        public DbSet<ConversationPlanner.Models.Conversation> Conversation { get; set; }
    }
}
