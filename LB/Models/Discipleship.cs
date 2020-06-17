using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class DiscipleshipGroup
	{
		public DiscipleshipGroup()
		{
			Members = new List<DiscipleshipMember>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		[DefaultValue(false)]
		public bool Deleted { get; set; }
		public virtual List<DiscipleshipMember> Members { get; set; }
	}

	public class DiscipleshipMember
	{
		public Guid MemberId { get; set; }
		public Member Member { get; set; }
		public int DiscipleshipGroupId { get; set; }
		public DiscipleshipGroup DiscipleshipGroup { get; set; }
	}


	public enum DiscipleshipType
	{
		Online, Offline
	}

	public class DiscipleshipRegistration
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public Guid MemberId { get; set; }
		public Member Member { get; set; }
		public DiscipleshipType Type { get; set; }
		public DateTime Created { get; set; }
		[DefaultValue(false)]
		public bool Completed { get; set; }
	}
}
