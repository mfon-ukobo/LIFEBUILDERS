using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{

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
