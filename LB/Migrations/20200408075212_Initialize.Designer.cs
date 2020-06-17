﻿// <auto-generated />
using System;
using LB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LB.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200408075212_Initialize")]
    partial class Initialize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LB.Models.AdminTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AdminUserId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("Finished");

                    b.HasKey("Id");

                    b.HasIndex("AdminUserId");

                    b.ToTable("AdminTasks");
                });

            modelBuilder.Entity("LB.Models.AdminUser", b =>
                {
                    b.Property<Guid>("AppUserId");

                    b.Property<DateTime>("LastLogin");

                    b.HasKey("AppUserId");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("LB.Models.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LB.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LB.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Body");

                    b.Property<int>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("LB.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("LB.Models.DiscipleshipGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DiscipleshipGroups");
                });

            modelBuilder.Entity("LB.Models.DiscipleshipMember", b =>
                {
                    b.Property<Guid>("MemberId");

                    b.Property<int>("DiscipleshipGroupId");

                    b.HasKey("MemberId", "DiscipleshipGroupId");

                    b.HasIndex("DiscipleshipGroupId");

                    b.ToTable("DiscipleshipMembers");
                });

            modelBuilder.Entity("LB.Models.DiscipleshipRegistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Completed");

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("MemberId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("DiscipleshipRegistrations");
                });

            modelBuilder.Entity("LB.Models.EventRegistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("LBEventId");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("LBEventId");

                    b.ToTable("EventRegistrations");
                });

            modelBuilder.Entity("LB.Models.FrontPageCarousel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ButtonLink");

                    b.Property<string>("ButtonText");

                    b.Property<string>("Caption")
                        .HasMaxLength(35);

                    b.Property<DateTime>("Created");

                    b.Property<string>("Details");

                    b.Property<bool>("HasButton");

                    b.Property<int>("ImageId");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("FrontPageCarousels");
                });

            modelBuilder.Entity("LB.Models.GalleryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption");

                    b.Property<DateTime>("Created");

                    b.Property<int>("ImageId");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.ToTable("Gallery");
                });

            modelBuilder.Entity("LB.Models.LBEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Body");

                    b.Property<string>("Caption");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Deleted");

                    b.Property<string>("EndDate");

                    b.Property<string>("EndTime");

                    b.Property<int>("ImageId");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("Published");

                    b.Property<string>("StartDate")
                        .IsRequired();

                    b.Property<string>("StartTime")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Venue")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Events");
                });

            modelBuilder.Entity("LB.Models.MailList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MailLists");
                });

            modelBuilder.Entity("LB.Models.MailListMember", b =>
                {
                    b.Property<int>("MailListId");

                    b.Property<Guid>("MemberId");

                    b.HasKey("MailListId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("MailListMember");
                });

            modelBuilder.Entity("LB.Models.Member", b =>
                {
                    b.Property<Guid>("AppUserId");

                    b.Property<int>("CountryId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("LastLogin");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Responsibility");

                    b.HasKey("AppUserId");

                    b.HasIndex("CountryId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("LB.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsRead");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("LB.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Description");

                    b.Property<bool>("IsSeen");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("LB.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AudioUrl");

                    b.Property<string>("Author");

                    b.Property<string>("Body");

                    b.Property<string>("Caption")
                        .IsRequired();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Deleted");

                    b.Property<int?>("ImageId");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("Published");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Type");

                    b.Property<string>("VideoUrl");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("LB.Models.PostCategory", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("CategoryId");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("LB.Models.PostTag", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("TagId");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("LB.Models.Reply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Body");

                    b.Property<int?>("CommentId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("LB.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<int?>("ImageId");

                    b.Property<bool>("IsLocalResource");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("ResourceUrl");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ImageId");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("LB.Models.ResourceAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ResourceAccesses");
                });

            modelBuilder.Entity("LB.Models.ResourceAccessMember", b =>
                {
                    b.Property<Guid>("MemberId");

                    b.Property<int>("ResourceAccessId");

                    b.HasKey("MemberId", "ResourceAccessId");

                    b.HasIndex("ResourceAccessId");

                    b.ToTable("ResourceAccessMember");
                });

            modelBuilder.Entity("LB.Models.ResourceAccessRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Finished");

                    b.Property<Guid>("MemberId");

                    b.Property<string>("Reason");

                    b.Property<int>("ResourceId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ResourceAccessRequests");
                });

            modelBuilder.Entity("LB.Models.ResourceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ResourceCategories");
                });

            modelBuilder.Entity("LB.Models.ResourceDownload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("MemberId");

                    b.Property<int>("ResourceId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ResourceDownloads");
                });

            modelBuilder.Entity("LB.Models.ResourceToAccess", b =>
                {
                    b.Property<int>("ResourceId");

                    b.Property<int>("ResourceAccessId");

                    b.HasKey("ResourceId", "ResourceAccessId");

                    b.HasIndex("ResourceAccessId");

                    b.ToTable("ResourceToAccess");
                });

            modelBuilder.Entity("LB.Models.SiteImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alt");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Image");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Thumbnail");

                    b.HasKey("Id");

                    b.ToTable("SiteImages");
                });

            modelBuilder.Entity("LB.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LB.Models.AdminTask", b =>
                {
                    b.HasOne("LB.Models.AdminUser", "AdminUser")
                        .WithMany("AdminTasks")
                        .HasForeignKey("AdminUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.AdminUser", b =>
                {
                    b.HasOne("LB.Models.AppUser", "AppUser")
                        .WithOne("AdminUser")
                        .HasForeignKey("LB.Models.AdminUser", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.Comment", b =>
                {
                    b.HasOne("LB.Models.Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.DiscipleshipMember", b =>
                {
                    b.HasOne("LB.Models.DiscipleshipGroup", "DiscipleshipGroup")
                        .WithMany("Members")
                        .HasForeignKey("DiscipleshipGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Member", "Member")
                        .WithMany("Groups")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.DiscipleshipRegistration", b =>
                {
                    b.HasOne("LB.Models.Member", "Member")
                        .WithMany("DiscipleshipRegistrations")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.EventRegistration", b =>
                {
                    b.HasOne("LB.Models.LBEvent", "LBEvent")
                        .WithMany("Registrations")
                        .HasForeignKey("LBEventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.FrontPageCarousel", b =>
                {
                    b.HasOne("LB.Models.SiteImage", "Image")
                        .WithMany("FrontPageCarousels")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.GalleryItem", b =>
                {
                    b.HasOne("LB.Models.SiteImage", "Image")
                        .WithOne("GalleryImage")
                        .HasForeignKey("LB.Models.GalleryItem", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.LBEvent", b =>
                {
                    b.HasOne("LB.Models.SiteImage", "Image")
                        .WithMany("Events")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.MailListMember", b =>
                {
                    b.HasOne("LB.Models.MailList", "MailList")
                        .WithMany("Members")
                        .HasForeignKey("MailListId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Member", "Member")
                        .WithMany("MailLists")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.Member", b =>
                {
                    b.HasOne("LB.Models.AppUser", "AppUser")
                        .WithOne("Member")
                        .HasForeignKey("LB.Models.Member", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Country", "Country")
                        .WithMany("Members")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.Post", b =>
                {
                    b.HasOne("LB.Models.SiteImage", "Image")
                        .WithMany("Posts")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("LB.Models.PostCategory", b =>
                {
                    b.HasOne("LB.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Post", "Post")
                        .WithMany("Categories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.PostTag", b =>
                {
                    b.HasOne("LB.Models.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Tag", "Tag")
                        .WithMany("Posts")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.Reply", b =>
                {
                    b.HasOne("LB.Models.Comment", "Comment")
                        .WithMany()
                        .HasForeignKey("CommentId");
                });

            modelBuilder.Entity("LB.Models.Resource", b =>
                {
                    b.HasOne("LB.Models.ResourceCategory", "Category")
                        .WithMany("Resources")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.SiteImage", "Image")
                        .WithMany("Resources")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("LB.Models.ResourceAccessMember", b =>
                {
                    b.HasOne("LB.Models.Member", "Member")
                        .WithMany("ResourceAccesses")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.ResourceAccess", "ResourceAccess")
                        .WithMany("Members")
                        .HasForeignKey("ResourceAccessId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.ResourceAccessRequest", b =>
                {
                    b.HasOne("LB.Models.Member", "Member")
                        .WithMany("ResourceAccessRequests")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Resource", "Resource")
                        .WithMany("ResourceAccessRequests")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.ResourceDownload", b =>
                {
                    b.HasOne("LB.Models.Member", "Member")
                        .WithMany("ResourceDownloads")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Resource", "Resource")
                        .WithMany("ResourceDownloads")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LB.Models.ResourceToAccess", b =>
                {
                    b.HasOne("LB.Models.ResourceAccess", "ResourceAccess")
                        .WithMany("Resources")
                        .HasForeignKey("ResourceAccessId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.Resource", "Resource")
                        .WithMany("ResourceAccesses")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("LB.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("LB.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LB.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("LB.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
