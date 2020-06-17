using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class PasswordResetView
	{
		public string UserId { get; set; }
		public string Code { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsPassword => Password.Equals(ConfirmPassword);
	}
}
