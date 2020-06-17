using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class LBEvent
	{
		public LBEvent()
		{
			if (this.Registrations == null)
			{
				this.Registrations = new List<EventRegistration>();
			}
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[MaxLength(255)]
		[Required]
		public string Title { get; set; }

		public string Slug => Utils.ConvertToSlug(Title);
		public string Caption { get; set; }
		public string Body { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public string StartDate { get; set; }
		[DataType(DataType.Date)]
		public string EndDate { get; set; }

		[Required]
		[DataType(DataType.Time)]
		public string StartTime { get; set; }
		[DataType(DataType.Time)]
		public string EndTime { get; set; }

		[Required]
		public string Venue { get; set; }

		[Required]
		public string Address { get; set; }
		
		public SiteImage Image { get; set; }
		public int ImageId { get; set; }

		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public bool Deleted { get; set; }

		[DefaultValue(1)]
		public Published Published { get; set; }

		public virtual ICollection<EventRegistration> Registrations { get; set; }
	}

	public class EventRegistration
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[DefaultValue(0)]
		public bool Deleted { get; set; }

		public int LBEventId { get; set; }
		public LBEvent LBEvent { get; set; }
	}
}
