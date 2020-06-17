using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public enum AppEmailTypes
	{
		Registration, Info
	}

	public class AppEmail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string EmailAddress { get; set; }
		public AppEmailTypes Type { get; set; }
		public string Description { get; set; }
	}
}
