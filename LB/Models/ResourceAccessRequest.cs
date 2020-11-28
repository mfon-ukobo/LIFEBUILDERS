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
