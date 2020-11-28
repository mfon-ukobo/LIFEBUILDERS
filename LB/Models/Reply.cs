using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{

	/// <summary>
	/// Post Comment Replies
	/// </summary>
	public class Reply
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Author { get; set; }
		public string Body { get; set; }
		public Comment Comment { get; set; }
	}
}
