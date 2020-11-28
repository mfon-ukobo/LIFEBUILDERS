using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class Sermon
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public SiteImage SiteImage { get; set; }
		public int? SiteImageId { get; set; }

		public string Description { get; set; }

		public string FileURL { get; set; }
		public FileType FileType { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime LastModified { get; set; }

		[DefaultValue(false)]
		public bool Published { get; set; }
		public bool Deleted { get; set; }
	}
}
