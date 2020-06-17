using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LB.Controllers
{
    public class BlogController : Controller
    {
		private readonly Context context;

		public BlogController(Context _context)
		{
			this.context = _context;
		}

        public async Task<IActionResult> Index(int? page, string tag, string category)
        {
			var posts = context.Posts.Include(x => x.Image).Include(x => x.Tags).Include(x => x.Categories).ThenInclude(c => c.Category).OrderByDescending(x => x.Modified).Where(x => x.Deleted == false && x.Published == Published.Publish);

			if (!string.IsNullOrEmpty(tag))
			{
				var searchTag = await context.Tags.FirstOrDefaultAsync(x => x.Slug == tag);
				posts = posts.Where(x => x.Tags.Select(t => t.TagId).Contains(searchTag.Id));
			}

			if (!string.IsNullOrEmpty(category))
			{
				var searchCategory = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category);
				posts = posts.Where(x => x.Categories.Select(t => t.CategoryId).Contains(searchCategory.Id));
			}

			return View(await PaginatedList<Post>.CreateAsync(posts, page ?? 1, 5));
        }

		public async Task<IActionResult> Search(int? page, string q)
		{
			var posts = context.Posts.Include(x => x.Image).Where(x => x.Deleted == false && x.Published == Published.Publish);
			posts = posts.Where(x => x.Title.ToLower().Contains(q.ToLower()));
			return View(nameof(Index), await PaginatedList<Post>.CreateAsync(posts, page ?? 1, 5));
		}

		[Route("/blog/{id}")]
		public IActionResult Single(string id)
		{
			Post post = context.Posts.Include(x => x.Image).Include(x => x.Categories).ThenInclude(x => x.Category).Include(x => x.Tags).ThenInclude(x => x.Tag).FirstOrDefault(x => x.Slug == id);
			return View(post);
		}
    }
}