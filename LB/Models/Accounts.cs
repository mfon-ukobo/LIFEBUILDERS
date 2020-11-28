using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public partial class AppUser : IdentityUser<Guid>
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string FullName => string.Format("{0} {1}", LastName, FirstName);
		[NotMapped]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[NotMapped]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsPassword => !(string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword)) ? Password.Equals(ConfirmPassword) : false;

		[NotMapped]
		public bool IsDiscipleshipRegistration { get; set; }

		public virtual AdminUser AdminUser { get; set; }
		public virtual Member Member { get; set; }
	}
}
