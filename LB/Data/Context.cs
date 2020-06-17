using LB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB
{
	public class Context : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
	{
		public Context(DbContextOptions<Context> options) : base(options) { }

		public DbSet<SiteImage> SiteImages { get; set; }
		public DbSet<GalleryItem> Gallery { get; set; }

		public DbSet<Post> Posts { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Reply> Replies { get; set; }
		public DbSet<LBEvent> Events { get; set; }
		public DbSet<EventRegistration> EventRegistrations { get; set; }
		public DbSet<Resource> Resources { get; set; }
		public DbSet<ResourceCategory> ResourceCategories { get; set; }
		public DbSet<ResourceAccess> ResourceAccesses { get; set; }
		public DbSet<ResourceDownload> ResourceDownloads { get; set; }
		public DbSet<ResourceAccessRequest> ResourceAccessRequests { get; set; }

		public DbSet<AdminUser> AdminUsers { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Country> Countries { get; set; }

		public DbSet<MailList> MailLists { get; set; }

		public DbSet<Notification> Notifications { get; set; }
		public DbSet<AdminTask> AdminTasks { get; set; }

		public DbSet<FrontPageCarousel> FrontPageCarousels { get; set; }

		public DbSet<DiscipleshipGroup> DiscipleshipGroups { get; set; }
		public DbSet<DiscipleshipMember> DiscipleshipMembers { get; set; }
		public DbSet<DiscipleshipRegistration> DiscipleshipRegistrations { get; set; }

		public DbSet<Message> Messages { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			// Front Page Carousel
			builder.Entity<FrontPageCarousel>()
				.HasOne(x => x.Image)
				.WithMany(x => x.FrontPageCarousels)
				.HasForeignKey(x => x.ImageId);


			// Discipleship
			builder.Entity<DiscipleshipMember>()
				.HasOne(x => x.DiscipleshipGroup)
				.WithMany(x => x.Members)
				.HasForeignKey(x => x.DiscipleshipGroupId);

			builder.Entity<DiscipleshipMember>()
				.HasOne(x => x.Member)
				.WithMany(x => x.Groups)
				.HasForeignKey(x => x.MemberId);

			builder.Entity<DiscipleshipMember>()
				.HasKey(x => new { x.MemberId, x.DiscipleshipGroupId });

			builder.Entity<DiscipleshipRegistration>()
				.HasOne(x => x.Member)
				.WithMany(x => x.DiscipleshipRegistrations)
				.HasForeignKey(x => x.MemberId);


			// Post Slug Index
			builder.Entity<Post>()
				.HasIndex(p => p.Title)
				.IsUnique();

			builder.Entity<Post>()
				.HasOne(x => x.Image)
				.WithMany(x => x.Posts)
				.HasForeignKey(x => x.ImageId);

			// PostCategory Slug Index
			builder.Entity<Category>()
				.HasIndex(x => x.Name)
				.IsUnique();

			// PostTag Slug Index
			builder.Entity<Tag>()
				.HasIndex(x => x.Name)
				.IsUnique();


			// Post Category and Tag Configuration
			builder.Entity<PostCategory>()
				.HasKey(x => new { x.PostId, x.CategoryId });

			builder.Entity<PostTag>()
				.HasKey(x => new { x.PostId, x.TagId });

			builder.Entity<PostCategory>()
				.HasOne(x => x.Post)
				.WithMany(p => p.Categories)
				.HasForeignKey(x => x.PostId);

			builder.Entity<PostCategory>()
				.HasOne(x => x.Category)
				.WithMany(p => p.Posts)
				.HasForeignKey(x => x.CategoryId);

			builder.Entity<PostTag>()
				.HasOne(x => x.Post)
				.WithMany(p => p.Tags)
				.HasForeignKey(x => x.PostId);

			builder.Entity<PostTag>()
				.HasOne(x => x.Tag)
				.WithMany(p => p.Posts)
				.HasForeignKey(x => x.TagId);


			// Events
			builder.Entity<LBEvent>()
				.HasIndex(x => x.Title)
				.IsUnique(true);

			builder.Entity<LBEvent>()
				.HasOne(x => x.Image)
				.WithMany(x => x.Events)
				.HasForeignKey(x => x.ImageId);

			builder.Entity<EventRegistration>()
				.HasOne(x => x.LBEvent)
				.WithMany(x => x.Registrations)
				.HasForeignKey(x => x.LBEventId);

			// Gallery Item
			builder.Entity<GalleryItem>()
				.HasOne(x => x.Image)
				.WithOne(x => x.GalleryImage)
				.HasForeignKey<GalleryItem>(x => x.ImageId);

			// Resource
			builder.Entity<Resource>()
				.HasOne(x => x.Category)
				.WithMany(x => x.Resources)
				.HasForeignKey(x => x.CategoryId);

			builder.Entity<ResourceCategory>()
				.HasIndex(x => x.Name)
				.IsUnique(true);

			builder.Entity<Resource>()
				.HasOne(x => x.Image)
				.WithMany(x => x.Resources)
				.HasForeignKey(x => x.ImageId);

			builder.Entity<Resource>()
				.HasIndex(x => x.Title)
				.IsUnique(true);

			// Resource to access

			builder.Entity<ResourceToAccess>()
				.HasOne(x => x.Resource)
				.WithMany(x => x.ResourceAccesses)
				.HasForeignKey(x => x.ResourceId);

			builder.Entity<ResourceToAccess>()
				.HasOne(x => x.ResourceAccess)
				.WithMany(x => x.Resources)
				.HasForeignKey(x => x.ResourceAccessId);

			builder.Entity<ResourceToAccess>()
				.HasKey(x => new { x.ResourceId, x.ResourceAccessId });


			// Resource Access to member

			builder.Entity<ResourceAccess>()
				.HasIndex(x => x.Name)
				.IsUnique(true);

			builder.Entity<ResourceAccessMember>()
				.HasOne(x => x.Member)
				.WithMany(x => x.ResourceAccesses)
				.HasForeignKey(x => x.MemberId);

			builder.Entity<ResourceAccessMember>()
				.HasOne(x => x.ResourceAccess)
				.WithMany(x => x.Members)
				.HasForeignKey(x => x.ResourceAccessId);

			builder.Entity<ResourceAccessMember>()
				.HasKey(x => new { x.MemberId, x.ResourceAccessId });


			// Resource Download
			builder.Entity<ResourceDownload>()
				.HasOne(x => x.Member)
				.WithMany(x => x.ResourceDownloads)
				.HasForeignKey(x => x.MemberId);

			builder.Entity<ResourceDownload>()
				.HasOne(x => x.Resource)
				.WithMany(x => x.ResourceDownloads)
				.HasForeignKey(x => x.ResourceId);

			// Resource Access Requests
			builder.Entity<ResourceAccessRequest>()
				.HasOne(x => x.Member)
				.WithMany(x => x.ResourceAccessRequests)
				.HasForeignKey(x => x.MemberId);

			builder.Entity<ResourceAccessRequest>()
				.HasOne(x => x.Resource)
				.WithMany(x => x.ResourceAccessRequests)
				.HasForeignKey(x => x.ResourceId);


			// Users
			builder.Entity<AdminUser>()
				.HasOne(x => x.AppUser)
				.WithOne(x => x.AdminUser)
				.HasForeignKey<AdminUser>(x => x.AppUserId);

			builder.Entity<AdminUser>()
				.HasKey(x => x.AppUserId);

			builder.Entity<Member>()
				.HasOne(x => x.AppUser)
				.WithOne(x => x.Member)
				.HasForeignKey<Member>(x => x.AppUserId);

			builder.Entity<Member>()
				.HasKey(x => x.AppUserId);

			builder.Entity<Member>()
				.HasOne(x => x.Country)
				.WithMany(x => x.Members)
				.HasForeignKey(x => x.CountryId);


			// country
			builder.Entity<Country>()
				.HasIndex(x => x.Name)
				.IsUnique(true);

			// Mail List
			builder.Entity<MailListMember>()
				.HasOne(x => x.MailList)
				.WithMany(x => x.Members)
				.HasForeignKey(x => x.MailListId);

			builder.Entity<MailListMember>()
				.HasOne(x => x.Member)
				.WithMany(x => x.MailLists)
				.HasForeignKey(x => x.MemberId);

			builder.Entity<MailListMember>()
				.HasKey(x => new { x.MailListId, x.MemberId });


			// Admin Task
			builder.Entity<AdminTask>()
				.HasOne(x => x.AdminUser)
				.WithMany(x => x.AdminTasks)
				.HasForeignKey(x => x.AdminUserId);

			//Seed


			base.OnModelCreating(builder);
		}
	}
}
