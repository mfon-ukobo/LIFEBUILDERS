using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{

	/// <summary>
	/// Blog Categories
	/// </summary>
	public class Category
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[MaxLength(50)]
		[Required]
		public string Name { get; set; }
		public string Slug => Utils.ConvertToSlug(Name);
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public List<PostCategory> Posts { get; set; }
	}
}
