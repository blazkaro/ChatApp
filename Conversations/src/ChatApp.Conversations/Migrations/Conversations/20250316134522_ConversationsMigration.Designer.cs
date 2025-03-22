﻿// <auto-generated />
using System;
using ChatApp.Conversations.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatApp.Conversations.Migrations.Conversations
{
    [DbContext(typeof(ConversationsDbContext))]
    [Migration("20250316134522_ConversationsMigration")]
    partial class ConversationsMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChatApp.Conversations.Db.Entities.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("ChatApp.Conversations.Db.Entities.ConversationInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<string>("InvitedUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("InvitedUserId");

                    b.ToTable("ConversationInvitations");
                });

            modelBuilder.Entity("ChatApp.Conversations.Db.Entities.ConversationMember", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.HasKey("UserId", "ConversationId");

                    b.HasIndex("ConversationId");

                    b.ToTable("ConversationMember");
                });

            modelBuilder.Entity("ChatApp.Conversations.Db.Entities.ConversationInvitation", b =>
                {
                    b.HasOne("ChatApp.Conversations.Db.Entities.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("ChatApp.Conversations.Db.Entities.ConversationMember", b =>
                {
                    b.HasOne("ChatApp.Conversations.Db.Entities.Conversation", "Conversation")
                        .WithMany("Members")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("ChatApp.Conversations.Db.Entities.Conversation", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
