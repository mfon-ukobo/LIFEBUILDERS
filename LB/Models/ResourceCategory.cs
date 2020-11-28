using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LB.Models
{
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
}
