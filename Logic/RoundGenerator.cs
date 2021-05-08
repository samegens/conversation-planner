using ConversationPlanner.Models;
using ConversationPlanner.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConversationPlanner.Logic
{
    public class RoundGenerator
    {
        public List<Conversation> Generate(List<Participant> participants, List<Conversation> previousConversations, DateTime timestamp)
        {
            previousConversations ??= new List<Conversation>();

            // Only the conversations on the same day are relevant to filter out conversation candidates.
            DateTime date = timestamp.Date;
            previousConversations = previousConversations.Where(c => c.Timestamp.Date == date).ToList();

            var presentParticipants = GetPresentParticipants(participants);
            var fillers = new List<Participant>();
            var conversations = new List<Conversation>();
            while (presentParticipants.Count > 0)
            {
                var participant1 = presentParticipants[0];
                var participant2 = SelectConversationPartnerFor(participant1, presentParticipants, previousConversations);
                if (participant2 != null)
                {
                    var conversation = new Conversation
                    {
                        Participant1 = participant1,
                        Participant2 = participant2,
                        Timestamp = timestamp
                    };
                    conversations.Add(conversation);

                    presentParticipants.Remove(participant1);
                    presentParticipants.Remove(participant2);
                }
                else
                {
                    fillers.Add(participant1);
                    presentParticipants.Remove(participant1);
                }
            }

            fillers.AddRange(participants.Where(p => p.IsPresent && string.IsNullOrEmpty(p.Tag)));
            while (fillers.Count > 1)
            {
                var conversation = new Conversation
                {
                    Participant1 = fillers[0],
                    Participant2 = fillers[1],
                    Timestamp = timestamp
                };
                conversations.Add(conversation);

                fillers.Remove(conversation.Participant1);
                fillers.Remove(conversation.Participant2);
            }

            // This may leave one person without a conversation, we accept this for now.

            return conversations;
        }

        private Participant SelectConversationPartnerFor(Participant participant, List<Participant> presentParticipants, List<Conversation> previousConversations)
        {
            return GetConversationCandidatesFor(participant, presentParticipants, previousConversations).FirstOrDefault();
        }

        /// <summary>
        /// Return the present non-filler participants, randomly shuffled.
        /// </summary>
        private List<Participant> GetPresentParticipants(List<Participant> participants)
        {
            var presentParticipants = participants.Where(p => !string.IsNullOrEmpty(p.Tag)).ToList();
            presentParticipants.Shuffle();
            return presentParticipants;
        }

        private List<Participant> GetConversationCandidatesFor(Participant participant, IEnumerable<Participant> presentParticipants, List<Conversation> previousConversations)
        {
            var candidates = presentParticipants.Where(p => p.Tag != participant.Tag).ToList();
            var previousConversationPartners = previousConversations.Where(c => c.Participant1 == participant).Select(c => c.Participant2)
                .Union(previousConversations.Where(c => c.Participant2 == participant).Select(c => c.Participant1))
                .ToList();
            candidates = candidates.Except(previousConversationPartners).ToList();

            // No one left? Then drop the requirement that the tag should be different.
            if (candidates.Count == 0)
            {
                candidates = presentParticipants.Where(p => p != participant).ToList();
                candidates = candidates.Except(previousConversationPartners).ToList();
            }

            // Still no one left? Then drop all requirements.
            if (candidates.Count == 0)
            {
                candidates = presentParticipants.Where(p => p != participant).ToList();
            }

            candidates.Shuffle();

            return candidates;
        }
    }
}
