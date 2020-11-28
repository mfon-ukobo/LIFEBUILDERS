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
	/// Main Resource
	/// </summary>
	public class Resource
	{
		public Resource()
		{
			ResourceAccesses = new List<ResourceToAccess>();
			ResourceDownloads = new List<ResourceDownload>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Slug => Utils.ConvertToSlug(Title);
		public string Description { get; set; }
		public SiteImage Image { get; set; }
		public int? ImageId { get; set; }
		[Display(Name = "Resource URL")]
		public string ResourceUrl { get; set; }
		public bool IsLocalResource { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		[NotMapped]
		public IFormFile LocalFile { get; set; }

		public ResourceCategory Category { get; set; }
		[Required]
		public int CategoryId { get; set; }

		public bool HasImage => Image != null;

		[DefaultValue(false)]
		public bool Deleted { get; set; }

		public virtual List<ResourceToAccess> ResourceAccesses { get; set; }
		public virtual List<ResourceDownload> ResourceDownloads { get; set; }
		public virtual List<ResourceAccessRequest> ResourceAccessRequests { get; set; }

		//[DefaultValue(false)]
		//public bool RequiresLogin { get; set; }


		[NotMapped]
		public string[] SelectedAccesses { get; set; }
	}
}
