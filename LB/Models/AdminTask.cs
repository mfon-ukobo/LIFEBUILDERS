using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LB.Models
{

	public class AdminTask
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Description { get; set; }
		public DateTime Created { get; set; }
		public bool Finished { get; set; }
		public AdminUser AdminUser { get; set; }
		public Guid AdminUserId { get; set; }
		public string CreatedString => Created.ToString("MMM dd hh:mmtt");
	}
}
