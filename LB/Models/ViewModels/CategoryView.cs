using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models.ViewModels
{
	public class CategoryView
	{
		public Category Category { get; set; }
		public bool IsEditing { get; set; }
		public PaginatedList<Category> Categories { get; set; }
	}
}
