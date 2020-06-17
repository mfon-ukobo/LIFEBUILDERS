using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{

	public enum BulkActionsIndex
	{
		Delete
	}

	public enum BulkActionsTrash
	{
		Restore, Delete
	}

	public class FrontPageCarousel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[MaxLength(35)]
		public string Caption { get; set; }
		public string Details { get; set; }
		public bool HasButton { get; set; }
		public string ButtonText { get; set; }
		public string ButtonLink { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public SiteImage Image { get; set; }
		public int ImageId { get; set; }
	}

	public class Donation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public string Amount { get; set; }
		public DateTime Created { get; set; }
	}

	public class Message
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public string Body { get; set; }
		public DateTime Created { get; set; }
		[DefaultValue(false)]
		public bool IsRead { get; set; }
	}
}
