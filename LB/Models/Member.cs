using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{

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
}
