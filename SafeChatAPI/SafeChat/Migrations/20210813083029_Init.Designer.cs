﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SafeChatAPI.Data.AppDbContext;

namespace SafeChatAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210813083029_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GroupEntityUserEntity", b =>
                {
                    b.Property<int>("GroupsMemberId")
                        .HasColumnType("int");

                    b.Property<int>("MembersId")
                        .HasColumnType("int");

                    b.HasKey("GroupsMemberId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("GroupEntityUserEntity");
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.GroupAdminEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupsAdmins");
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.GroupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsGroupPublic")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.GroupInvitationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int?>("InvitedUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("InvitedUserId");

                    b.ToTable("GroupInvitations");
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.ImageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MessageEntityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessageEntityId");

                    b.ToTable("ImageEntity");
                });

            modelBuilder.Entity("SafeChatAPI.Data.GroupBanEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BannedForGroupId")
                        .HasColumnType("int");

                    b.Property<int?>("BannedUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BannedForGroupId");

                    b.HasIndex("BannedUserId");

                    b.ToTable("GroupBans");
                });

            modelBuilder.Entity("SafeChatAPI.Data.MessageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("SafeChatAPI.Data.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SafeChatAPI.Data.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GroupEntityUserEntity", b =>
                {
                    b.HasOne("SafeChatAPI.Data.AppDbContext.GroupEntity", null)
                        .WithMany()
                        .HasForeignKey("GroupsMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SafeChatAPI.Data.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.GroupAdminEntity", b =>
                {
                    b.HasOne("SafeChatAPI.Data.UserEntity", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("SafeChatAPI.Data.AppDbContext.GroupEntity", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.Navigation("Admin");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.GroupInvitationEntity", b =>
                {
                    b.HasOne("SafeChatAPI.Data.AppDbContext.GroupEntity", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("SafeChatAPI.Data.UserEntity", "InvitedUser")
                        .WithMany()
                        .HasForeignKey("InvitedUserId");

                    b.Navigation("Group");

                    b.Navigation("InvitedUser");
                });

            modelBuilder.Entity("SafeChatAPI.Data.AppDbContext.ImageEntity", b =>
                {
                    b.HasOne("SafeChatAPI.Data.MessageEntity", null)
                        .WithMany("Images")
                        .HasForeignKey("MessageEntityId");
                });

            modelBuilder.Entity("SafeChatAPI.Data.GroupBanEntity", b =>
                {
                    b.HasOne("SafeChatAPI.Data.AppDbContext.GroupEntity", "BannedForGroup")
                        .WithMany()
                        .HasForeignKey("BannedForGroupId");

                    b.HasOne("SafeChatAPI.Data.UserEntity", "BannedUser")
                        .WithMany()
                        .HasForeignKey("BannedUserId");

                    b.Navigation("BannedForGroup");

                    b.Navigation("BannedUser");
                });

            modelBuilder.Entity("SafeChatAPI.Data.MessageEntity", b =>
                {
                    b.HasOne("SafeChatAPI.Data.AppDbContext.GroupEntity", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SafeChatAPI.Data.RefreshToken", b =>
                {
                    b.HasOne("SafeChatAPI.Data.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SafeChatAPI.Data.MessageEntity", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
