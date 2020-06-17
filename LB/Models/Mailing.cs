using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class MailList
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Name { get; set; }
		public string Slug => Utils.ConvertToSlug(Name);
		public string Description { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public List<MailListMember> Members { get; set; }
	}

	public class MailListMember
	{
		public Guid MemberId { get; set; }
		public Member Member { get; set; }
		public int MailListId { get; set; }
		public MailList MailList { get; set; }
	}
}
