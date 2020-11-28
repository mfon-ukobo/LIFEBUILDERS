using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class PostCategory
	{
		public int PostId { get; set; }
		public Post Post { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
