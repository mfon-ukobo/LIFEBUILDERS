using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models.ViewModels
{
	public class TagView
	{
		public Tag Tag { get; set; }
		public bool IsEditing { get; set; }
		public PaginatedList<Tag> Tags { get; set; }
	}
}
