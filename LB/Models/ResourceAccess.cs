using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace LB.Models
{


	/// <summary>
	/// Resource Access
	/// </summary>
	public class ResourceAccess
	{
		public ResourceAccess()
		{
			Members = new List<ResourceAccessMember>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Slug => Utils.ConvertToSlug(Name);
		public string Description { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public virtual List<ResourceToAccess> Resources { get; set; }
		public virtual List<ResourceAccessMember> Members { get; set; }


		[NotMapped]
		public string[] SelectedMembers { get; set; }
	}
}
