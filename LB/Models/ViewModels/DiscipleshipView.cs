using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models.ViewModels
{
	public class DiscipleshipMemberRegistration
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public DiscipleshipType? Type { get; set; }
	}
}
