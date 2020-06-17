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


		[NotMapped]
		public string[] SelectedAccesses { get; set; }
	}


	/// <summary>
	/// Resource Category
	/// </summary>
	public class ResourceCategory
	{
		public ResourceCategory()
		{
			Resources = new List<Resource>();
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

		public virtual List<Resource> Resources { get; set; }
	}


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

	/// <summary>
	/// Resource to ResourceAccess Mapping
	/// </summary>
	public class ResourceToAccess
	{
		public int ResourceId { get; set; }
		public Resource Resource { get; set; }
		public ResourceAccess ResourceAccess { get; set; }
		public int ResourceAccessId { get; set; }
	}

	/// <summary>
	/// Member to ResourceAccess Mapping
	/// </summary>
	public class ResourceAccessMember
	{
		public Guid MemberId { get; set; }
		public Member Member { get; set; }
		public ResourceAccess ResourceAccess { get; set; }
		public int ResourceAccessId { get; set; }
	}

	/// <summary>
	/// Resource Download Logs
	/// </summary>
	public class ResourceDownload
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public Guid MemberId { get; set; }
		public Member Member { get; set; }
		public int ResourceId { get; set; }
		public Resource Resource { get; set; }
		public DateTime Created { get; set; }
	}

	/// <summary>
	/// Resource Access Request
	/// </summary>
	public class ResourceAccessRequest
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public Guid MemberId { get; set; }
		public Member Member { get; set; }
		public int ResourceId { get; set; }
		public Resource Resource { get; set; }
		public string Reason { get; set; }
		public DateTime Created { get; set; }
		public bool Finished { get; set; }
	}
}
