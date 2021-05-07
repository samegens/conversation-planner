﻿// <auto-generated />
using System;
using ConversationPlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConversationPlanner.Migrations
{
    [DbContext(typeof(ConversationPlannerContext))]
    [Migration("20210506143532_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13");

            modelBuilder.Entity("ConversationPlanner.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Participant1Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Participant2Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Participant1Id");

                    b.HasIndex("Participant2Id");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("ConversationPlanner.Models.Participant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Participant");
                });

            modelBuilder.Entity("ConversationPlanner.Models.Conversation", b =>
                {
                    b.HasOne("ConversationPlanner.Models.Participant", "Participant1")
                        .WithMany()
                        .HasForeignKey("Participant1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConversationPlanner.Models.Participant", "Participant2")
                        .WithMany()
                        .HasForeignKey("Participant2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
