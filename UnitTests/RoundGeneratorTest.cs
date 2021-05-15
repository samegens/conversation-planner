using System;
using System.Collections.Generic;
using ConversationPlanner.Logic;
using ConversationPlanner.Models;
using NUnit.Framework;
using FluentAssertions;

namespace ConversationPlanner.UnitTests
{
    [TestFixture]
    public class RoundGeneratorTest
    {
        private readonly RoundGenerator _sut = new RoundGenerator();

        [Test]
        public void TestGenerateWithoutParticipants()
        {
            // Arrange
            var participants = new List<Participant>();
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);

            // Act
            var conversations = _sut.Generate(participants, null, timestamp);

            // Assert
            Assert.That(conversations.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGenerateWithAbsentParticipants()
        {
            // Arrange
            var participants = new List<Participant>
            {
                CreateParticipant("Alice", "A", false),
                CreateParticipant("Bob", "A", false),
                CreateParticipant("Charlie", "A", false)
            };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);

            // Act
            var conversations = _sut.Generate(participants, null, timestamp);

            // Assert
            Assert.That(conversations.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGenerateWithParticipantsWithMatchingTags()
        {
            // Arrange
            var alice = CreateParticipant("Alice", "A", true);
            var bob = CreateParticipant("Bob", "A", true);
            var participants = new List<Participant>
            {
                alice,
                bob
            };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);

            for (int i = 0; i < 1000; i++)
            {
                // Act
                var conversations = _sut.Generate(participants, null, timestamp);

                // Assert
                Assert.That(conversations.Count, Is.EqualTo(1));
                var conversation = conversations[0];
                Assert.That(conversation.Participant1 == alice || conversation.Participant1 == bob);
                Assert.That(conversation.Participant1 != conversation.Participant2);
                Assert.IsNotNull(conversation.Participant1);
                Assert.IsNotNull(conversation.Participant2);
            }
        }

        [Test]
        public void TestGenerateWithParticipantsWithOneMismatchingTag()
        {
            // Arrange
            var alice = CreateParticipant("Alice", "A", true);
            var bob = CreateParticipant("Bob", "B", true);
            var participants = new List<Participant> { alice, bob };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);

            for (int i = 0; i < 1000; i++)
            {
                // Act
                var conversations = _sut.Generate(participants, null, timestamp);

                // Assert
                Assert.That(conversations.Count, Is.EqualTo(1));
                var conversation = conversations[0];
                Assert.That(conversation.Participant1 == alice || conversation.Participant1 == bob);
                Assert.That(conversation.Participant1 != conversation.Participant2);
                Assert.IsNotNull(conversation.Participant1);
                Assert.IsNotNull(conversation.Participant2);
            }
        }

        [Test]
        public void TestGenerateWithParticipantsWithTwoMismatchingTags()
        {
            // Arrange
            var alice = CreateParticipant("Alice", "A", true);
            var bob = CreateParticipant("Bob", "A", true);
            var charlie = CreateParticipant("Charlie", "B", true);
            var dan = CreateParticipant("Dan", "B", true);
            var participants = new List<Participant> { alice, bob, charlie, dan };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);

            for (int i = 0; i < 1000; i++)
            {
                // Act
                var conversations = _sut.Generate(participants, null, timestamp);

                // Assert
                Assert.That(conversations.Count, Is.EqualTo(2));

                foreach (var conversation in conversations)
                {
                    Assert.That(conversation.Participant1 == alice ? (conversation.Participant2 != bob) : true);
                    Assert.That(conversation.Participant1 == bob ? (conversation.Participant2 != alice) : true);
                    Assert.That(conversation.Participant1 == charlie ? (conversation.Participant2 != dan) : true);
                    Assert.That(conversation.Participant1 == dan ? (conversation.Participant2 != charlie) : true);
                    Assert.That(conversation.Participant1 != conversation.Participant2);
                    Assert.IsNotNull(conversation.Participant1);
                    Assert.IsNotNull(conversation.Participant2);
                }
            }
        }

        [Test]
        public void TestGenerateWithParticipantsWithMismatchingTagAndFreeParticipant()
        {
            // Arrange
            var alice = CreateParticipant("Alice", "A", true);
            var bob = CreateParticipant("Bob", null, true);
            var charlie = CreateParticipant("Charlie", "A", true);
            var dan = CreateParticipant("Dan", "B", true);
            var participants = new List<Participant> { alice, bob, charlie, dan };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);

            for (int i = 0; i < 1000; i++)
            {
                // Act
                var conversations = _sut.Generate(participants, null, timestamp);

                // Assert
                Assert.That(conversations.Count, Is.EqualTo(2));

                var conversation = conversations[0];
                Assert.That(conversation.Participant1 == dan || conversation.Participant2 == dan);
                Assert.That(conversation.Participant1 != conversation.Participant2);
                Assert.IsNotNull(conversation.Participant1);
                Assert.IsNotNull(conversation.Participant2);

                conversation = conversations[1];
                Assert.That(conversation.Participant1 == bob || conversation.Participant2 == bob);
                Assert.That(conversation.Participant1 != conversation.Participant2);
                Assert.IsNotNull(conversation.Participant1);
                Assert.IsNotNull(conversation.Participant2);
            }
        }

        [Test]
        public void TestGenerateWithParticipantsWithMismatchingTagAndPreviousConversation()
        {
            // Arrange
            var alice = CreateParticipant("Alice", "A", true);
            var bob = CreateParticipant("Bob", "B", true);
            var charlie = CreateParticipant("Charlie", "C", true);
            var participants = new List<Participant> { alice, bob, charlie };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);
            var previousConversations = new List<Conversation>
            {
                CreateConversation(alice, bob, timestamp.AddMinutes(-10))
            };

            for (int i = 0; i < 1000; i++)
            {
                // Act
                var conversations = _sut.Generate(participants, previousConversations, timestamp);

                // Assert
                Assert.That(conversations.Count, Is.EqualTo(1));

                var conversation = conversations[0];
                Assert.That(conversation.Participant1 == alice ? conversation.Participant2 == charlie : true);
                Assert.That(conversation.Participant1 == bob ? conversation.Participant2 == charlie : true);
                Assert.That(conversation.Participant1 != conversation.Participant2);
                Assert.IsNotNull(conversation.Participant1);
                Assert.IsNotNull(conversation.Participant2);
            }
        }

        [Test]
        public void TestGenerateWithParticipantsWithMatchingTagAndPreviousConversations()
        {
            // Arrange
            var alice = CreateParticipant("Alice", "A", true);
            var bob = CreateParticipant("Bob", "A", true);
            var charlie = CreateParticipant("Charlie", "B", true);
            var dan = CreateParticipant("Dan", "B", true);
            var participants = new List<Participant> { alice, bob, charlie, dan };
            var timestamp = new DateTime(2021, 5, 7, 17, 20, 0);
            var previousConversations = new List<Conversation>
            {
                CreateConversation(alice, charlie, timestamp.AddMinutes(-10)),
                CreateConversation(bob, dan, timestamp.AddMinutes(-10))
            };

            for (int i = 0; i < 1000; i++)
            {
                // Act
                var conversations = _sut.Generate(participants, previousConversations, timestamp);

                // Assert
                Assert.That(conversations.Count, Is.EqualTo(2));

                foreach (var conversation in conversations)
                {
                    if (conversation.Participant1 == alice)
                    {
                        conversation.Participant2.Should().BeEquivalentTo(dan);
                    }
                    if (conversation.Participant1 == bob)
                    {
                        conversation.Participant2.Should().BeEquivalentTo(charlie);
                    }
                    if (conversation.Participant1 == charlie)
                    {
                        conversation.Participant2.Should().BeEquivalentTo(bob);
                    }
                    if (conversation.Participant1 == dan)
                    {
                        conversation.Participant2.Should().BeEquivalentTo(alice);
                    }
                    Assert.That(conversation.Participant1 != conversation.Participant2);
                    Assert.IsNotNull(conversation.Participant1);
                    Assert.IsNotNull(conversation.Participant2);
                }
            }
        }

        private static Participant CreateParticipant(string name, string tag, bool isPresent)
        {
            return new Participant
            {
                Name = name,
                Tag = tag,
                IsPresent = isPresent
            };
        }

        private static Conversation CreateConversation(Participant participant1, Participant participant2, DateTime timestamp)
        {
            return new Conversation
            {
                Participant1 = participant1,
                Participant2 = participant2,
                Timestamp = timestamp
            };
        }
    }
}
