using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LB.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "Full Name")]
		public string Fullname { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare(nameof(Password), ErrorMessage = "Password and Confirmation do not match")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}
}
