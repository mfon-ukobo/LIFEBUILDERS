using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public partial class AppUser : IdentityUser<Guid>
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string FullName => string.Format("{0} {1}", LastName, FirstName);
		[NotMapped]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[NotMapped]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsPassword => !(string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword)) ? Password.Equals(ConfirmPassword) : false;

		[NotMapped]
		public bool IsDiscipleshipRegistration { get; set; }

		public virtual AdminUser AdminUser { get; set; }
		public virtual Member Member { get; set; }
	}

	public class AdminUser
	{
		public AdminUser()
		{
			AdminTasks = new List<AdminTask>();
		}
		public Guid AppUserId { get; set; }
		public virtual AppUser AppUser { get; set; }
		public DateTime LastLogin { get; set; }
		public virtual List<AdminTask> AdminTasks { get; set; }

		[NotMapped]
		public string[] SelectedRoles { get; set; }
	}

	public class Member
	{
		public Member()
		{
			ResourceDownloads = new List<ResourceDownload>();
		}

		public Guid AppUserId { get; set; }
		public virtual AppUser AppUser { get; set; }
		public int CountryId { get; set; }
		public Country Country { get; set; }
		public string Responsibility { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public DateTime LastLogin { get; set; }

		public virtual List<MailListMember> MailLists { get; set; }
		public virtual List<ResourceAccessMember> ResourceAccesses { get; set; }
		public virtual List<ResourceDownload> ResourceDownloads { get; set; }
		public virtual List<ResourceAccessRequest> ResourceAccessRequests { get; set; }
		public virtual List<DiscipleshipMember> Groups { get; set; }
		public List<DiscipleshipRegistration> DiscipleshipRegistrations { get; set; }

		[NotMapped]
		public string[] SelectedResourceAccess { get; set; }
	}

	public class Country
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Name { get; set; }
		public string Slug => Utils.ConvertToSlug(Name);
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public virtual List<Member> Members { get; set; }
	}
}
