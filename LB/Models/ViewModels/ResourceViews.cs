using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models.ViewModels
{
	/// <summary>
	/// Client Side Resource View
	/// </summary>
	public class ResourceView
	{
		public PaginatedList<ResourceCategory> ResourceCategories { get; set; }
		public List<Resource> Resources { get; set; }
	}



	public class ResourceCategoryView
	{
		public ResourceCategory ResourceCategory { get; set; }
		public bool IsEditing { get; set; }
		public PaginatedList<ResourceCategory> ResourceCategories { get; set; }
	}

	public class ResourceAccessView
	{
		public ResourceAccess ResourceAccess { get; set; }
		public PaginatedList<ResourceAccess> ResourceAccesses { get; set; }
		public bool IsEditing { get; set; }
	}
}
