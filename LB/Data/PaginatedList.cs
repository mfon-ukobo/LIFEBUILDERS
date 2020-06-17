using LB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB
{
	public class PaginatedList<T> : List<T>
	{
		public int PageIndex;
		public int TotalPages;
		public int PageSize;
		public int TotalItems;
		public List<T> CompleteList;

		public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, List<T> source)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			int temp = (int)Math.Ceiling(count / (double)pageSize);
			TotalPages = temp > 0 ? temp : 1;
			TotalItems = count;
			CompleteList = source;

			this.AddRange(items);
		}

		public int PageFirstIndex
		{
			get
			{
				if (TotalItems < 1)
				{
					return 0;
				}
				else
				{
					return (PageIndex - 1) * PageSize + 1;
				}
			}
		}

		public int PageLastIndex
		{
			get
			{
				var last = (PageFirstIndex + PageSize) - 1;
				if (last > TotalItems) last = TotalItems;
				return last;
			}
		}

		public bool HasPreviousPage
		{
			get
			{
				return (PageIndex > 1);
			}
		}

		public bool HasNextPage
		{
			get
			{
				return (PageIndex < TotalPages);
			}
		}

		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
		{
			var count = await source.CountAsync();
			var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PaginatedList<T>(items, count, pageIndex, pageSize, source.ToList());
		}

		public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
		{
			var count = source.Count();
			var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			return new PaginatedList<T>(items, count, pageIndex, pageSize, source.ToList());
		}
	}
}
