using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{

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
}
