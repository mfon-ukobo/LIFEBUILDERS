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
	/// Resource to ResourceAccess Mapping
	/// </summary>
	public class ResourceToAccess
	{
		public int ResourceId { get; set; }
		public Resource Resource { get; set; }
		public ResourceAccess ResourceAccess { get; set; }
		public int ResourceAccessId { get; set; }
	}
}
