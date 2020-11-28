using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LB.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LB.Models.ViewModels;

namespace LB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Root, Manager, Editor")]
	public class PostsController : Controller
    {
		private readonly Context context;
		private IHostingEnvironment env;
		private readonly UserManager<AppUser> userManager;
		public const int COUNT = 20;
		public const int MINCOUNT = 10;

		public PostsController(Context _context, IHostingEnvironment _env, UserManager<AppUser> _userManager)
		{
			context = _context;
			env = _env;
			userManager = _userManager;
		}

		[GetErrors]
		[Pagination]
        public async Task<IActionResult> Index(int? p, int? s, string search, Published? published)
        {
			var posts = context.Posts.Include(x => x.Categories).Include(x => x.Image).Where(x => !x.Deleted);

			if (search != null)
			{
				posts = posts.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
				ViewData["search"] = search;
			}

			if (published != null)
			{
				posts = posts.Where(x => x.Published == published);
				ViewData["published"] = published;
			}

			posts = posts.OrderByDescending(x => x.Modified);

			return View(await PaginatedList<Post>.CreateAsync(posts, p ?? 1, s ?? COUNT));
        }

		[GetErrors]
		public async Task<IActionResult> Trash(int? p, int? s, string search)
		{
			var posts = context.Posts.Include(x => x.Categories).Include(x => x.Image).Where(x => x.Deleted);

			if (search != null)
			{
				posts = posts.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
				ViewData["search"] = search;
			}

			posts = posts.OrderByDescending(x => x.Modified);
			return View(await PaginatedList<Post>.CreateAsync(posts, p ?? 1, s ?? COUNT));
		}

		public ActionResult Create()
		{
			ViewBag.Categories = context.Categories.ToList();
			ViewBag.Tags = context.Tags.ToList();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Post post)
		{
			if (post.SelectedCategories != null)
			{
				foreach (var s in post.SelectedCategories)
				{
					post.Categories.Add(new PostCategory
					{
						CategoryId = int.Parse(s)
					});
				}
			}

			if (post.SelectedTags != null)
			{
				foreach (var t in post.SelectedTags)
				{
					post.Tags.Add(new PostTag
					{
						TagId = int.Parse(t)
					});
				}
			}

			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(User.Identity.Name);
				post.Author = user.LastName + " " + user.FirstName;
				post.Created = DateTime.Now;
				post.Modified = DateTime.Now;
				post.Deleted = false;

				try
				{
					context.Posts.Add(post);
					context.SaveChanges();
					this.AddSessionError(ResponseStatus.Success, "Post created successfully!");
					return RedirectToAction(nameof(Index));
				}
				catch (MySqlException ex)
				{
					this.AddSessionError(ResponseStatus.Error, ex.InnerException.Message);
					ModelState.AddModelError("", ex.Message);
				}
			}
			ViewBag.Categories = context.Categories.ToList();
			ViewBag.Tags = context.Tags.ToList();
			return View(post);
		}

		[Route("/admin/posts/edit/{slug}")]
		[GetErrors]
		public async Task<ActionResult> Edit(string slug)
		{
			ViewBag.Categories = context.Categories.ToList();
			ViewBag.Tags = context.Tags.ToList();
			Post post = context.Posts.Include(x => x.Categories).Include(x => x.Tags).Include(x => x.Image).AsEnumerable().FirstOrDefault(x => x.Slug == slug);
			return View(post);
		}

		[HttpPost]
		[GetErrors]
		[Route("/admin/posts/edit/{slug}")]
		public async Task<ActionResult> Edit(int id)
		{
			Post post = await context.Posts.Include(x => x.Categories).Include(x => x.Tags).Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);
			string slug;

			if (ModelState.IsValid)
			{
				if (await TryUpdateModelAsync(post))
				{
					post.Categories = new List<PostCategory>();
					if (post.SelectedCategories != null)
					{
						foreach (var s in post.SelectedCategories)
						{
							post.Categories.Add(new PostCategory
							{
								CategoryId = int.Parse(s)
							});
						}
					}

					post.Tags = new List<PostTag>();
					if (post.SelectedTags != null)
					{
						foreach (var t in post.SelectedTags)
						{
							post.Tags.Add(new PostTag
							{
								TagId = int.Parse(t)
							});
						}
					}
					slug = post.Slug;
					post.Modified = DateTime.Now;
					try
					{
						await context.SaveChangesAsync();
						this.AddSessionError(ResponseStatus.Success, "Post updated successfully!");
						return RedirectToAction(nameof(Edit), new { slug });
					}
					catch (DbUpdateException ex)
					{
						this.AddSessionError(ResponseStatus.Error, $"Post Update Error: {ex.InnerException.Message}");
					}
				}
			}

			ViewBag.Categories = context.Categories.ToList();
			ViewBag.Tags = context.Tags.ToList();
			return View(post);
		}


		[HttpPost]
		public async Task<IActionResult> Delete(int id, int? p, int? s, string search)
		{
			var post = await context.Posts.FindAsync(id);
			try
			{
				post.Deleted = true;
				await context.SaveChangesAsync();
				this.AddSessionError(ResponseStatus.Success, "Delete Successful");
			}
			catch (DbUpdateException ex)
			{
				this.AddSessionError(ResponseStatus.Error, $"Delete Error: {ex.InnerException.Message}");
			}
			return RedirectToAction(nameof(Index), new { p, s, search });
		}

		[HttpPost]
		public async Task<IActionResult> Restore(int id)
		{
			var post = await context.Posts.FindAsync(id);
			try
			{
				post.Deleted = false;
				await context.SaveChangesAsync();
				this.AddSessionError(ResponseStatus.Success, "Restore Successful");
			}
			catch (DbUpdateException ex)
			{
				this.AddSessionError(ResponseStatus.Error, $"Restore Error: {ex.InnerException.Message}");
			}
			return RedirectToAction(nameof(Trash));
		}










		/// <summary>
		/// Categories
		/// </summary>
		/// <param name="id"></param>
		/// <param name="p"></param>
		/// <param name="s"></param>
		/// <param name="search"></param>
		/// <returns></returns>
		[GetErrors]
		public async Task<IActionResult> Categories(int? id, int? p, int? s, string search)
		{
			var model = new CategoryView
			{
				Categories = await PaginatedList<Category>.CreateAsync(CategoryList(search), p ?? 1, s ?? 10),
				Category = id.HasValue ? await context.Categories.FirstOrDefaultAsync(x => x.Id == id) : null,
				IsEditing = id.HasValue
			};
			
			return View(model);
		}
		public IQueryable<Category> CategoryList(string search= null)
		{
			var categories = context.Categories.OrderByDescending(x => x.Id).AsQueryable();

			if (search != null)
			{
				categories = categories.Where(x => x.Name.Match(search));
				ViewData["search"] = search;
			}

			return categories;
		}

		public IActionResult CreateCategory() => RedirectToAction(nameof(Categories));
		[HttpPost]
		[GetErrors]
		public async Task<IActionResult> CreateCategory(Category category)
		{
			if (ModelState.IsValid)
			{
				category.Created = category.Modified = DateTime.Now;
				try
				{
					context.Categories.Add(category);
					await context.SaveChangesAsync();
					return RedirectToAction(nameof(Categories));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			var model = new CategoryView
			{
				Categories = await PaginatedList<Category>.CreateAsync(context.Categories, 1, MINCOUNT),
				Category = category,
				IsEditing = false
			};

			return View(nameof(Categories), model);
		}

		[GetErrors]
		public async Task<IActionResult> EditCategory(int? id, int? p, int? s, string search)
		{
			var model = new CategoryView
			{
				Categories = await PaginatedList<Category>.CreateAsync(CategoryList(search), p ?? 1, s ?? MINCOUNT),
				Category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id),
				IsEditing = true
			};

			return View(nameof(Categories), model);
		}

		[GetErrors]
		[HttpPost]
		public async Task<IActionResult> EditCategory(int id, int? p, int? s, string search)
		{
			var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
			if (ModelState.IsValid)
			{
				if (await TryUpdateModelAsync(category))
				{
					category.Modified = DateTime.Now;
					try
					{
						await context.SaveChangesAsync();
						this.AddSessionError(ResponseStatus.Success, "Update Successful");
						return RedirectToAction(nameof(Categories), new { p, s, search });
					}
					catch (Exception ex)
					{
						this.AddSessionError(ResponseStatus.Error, "Update not Successful: " + ex.InnerException?.Message ?? ex.Message);
					}
				}
			}

			var model = new CategoryView
			{
				Categories = await PaginatedList<Category>.CreateAsync(CategoryList(search), p ?? 1, s ?? MINCOUNT),
				Category = category,
				IsEditing = true
			};

			return View(nameof(Categories), model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteCategory(int? id, int? p, int? s, string search)
		{
			if (!id.HasValue)
			{
				this.AddSessionError(ResponseStatus.Error, "Invalid Operation");
				return RedirectToAction(nameof(Categories), new { p, s, search });
			}

			var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

			try
			{
				context.Categories.Remove(category);
				await context.SaveChangesAsync();
				this.AddSessionError(ResponseStatus.Success, "Delete Successful");
			}
			catch (Exception ex)
			{
				this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
			}

			return RedirectToAction(nameof(Categories), new { p, s, search });
		}






		/// <summary>
		/// TAGS
		/// </summary>
		/// <param name="id"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		[GetErrors]
		public async Task<IActionResult> Tags(int? id, int? p, int? s, string search)
		{
			var tags = TagList(search);

			var model = new TagView
			{
				Tags = await PaginatedList<Tag>.CreateAsync(tags, p ?? 1, s ?? MINCOUNT),
				Tag = id.HasValue ? await context.Tags.FirstOrDefaultAsync(x => x.Id == id) : null,
				IsEditing = id.HasValue
			};

			return View(model);
		}
		public IQueryable<Tag> TagList(string search = null)
		{
			var tags = context.Tags.OrderByDescending(x => x.Id).AsQueryable();

			if (search != null)
			{
				tags = tags.Where(x => x.Name.Match(search));
			}

			return tags;
		}

		public IActionResult CreateTag() => RedirectToAction(nameof(Tags));
		[GetErrors]
		[HttpPost]
		public async Task<IActionResult> CreateTag(Tag tag)
		{
			if (ModelState.IsValid)
			{
				tag.Created = tag.Modified = DateTime.Now;
				try
				{
					context.Tags.Add(tag);
					await context.SaveChangesAsync();
					return RedirectToAction(nameof(Tags));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			var model = new TagView
			{
				Tags = await PaginatedList<Tag>.CreateAsync(TagList(), 1, MINCOUNT),
				Tag = tag,
				IsEditing = false
			};

			return View(nameof(Tags), model);
		}

		[GetErrors]
		public async Task<IActionResult> EditTag(int id, int? p, int? s, string search)
		{
			var tags = TagList(search);
			var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

			var model = new TagView
			{
				Tags = await PaginatedList<Tag>.CreateAsync(tags, p ?? 1, s ?? MINCOUNT),
				Tag = tag,
				IsEditing = true
			};

			return View(nameof(Tags), model);
		}

		[GetErrors]
		[HttpPost]
		[ActionName(nameof(EditTag))]
		public async Task<IActionResult> EditTagPost(int id, int? p, int? s, string search)
		{
			var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);
			if (ModelState.IsValid)
			{
				if (await TryUpdateModelAsync(tag))
				{
					tag.Modified = DateTime.Now;
					try
					{
						await context.SaveChangesAsync();
						this.AddSessionError(ResponseStatus.Success, "Update Successful");
						return RedirectToAction(nameof(Tags), new { p, s, search });
					}
					catch (Exception ex)
					{
						this.AddSessionError(ResponseStatus.Error, "Update not Successful: " + ex.InnerException?.Message ?? ex.Message);
					}
				}
			}

			var model = new TagView
			{
				Tags = await PaginatedList<Tag>.CreateAsync(TagList(search), p ?? 1, MINCOUNT),
				Tag = tag,
				IsEditing = true
			};

			return View(nameof(Tags), model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteTag(int? id, int? p, int? s, string search)
		{
			if (!id.HasValue)
			{
				this.AddSessionError(ResponseStatus.Error, "Invalid Operation");
				return RedirectToAction(nameof(Tags), new { p, s, search });
			}

			var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

			try
			{
				context.Tags.Remove(tag);
				await context.SaveChangesAsync();
				this.AddSessionError(ResponseStatus.Success, "Delete Successful");
			}
			catch (Exception ex)
			{
				this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
			}

			return RedirectToAction(nameof(Tags), new { p, s, search });
		}

		[HttpPost]
		public async Task<IActionResult> IndexBulkAction(BulkActionsIndex action, int[] id)
		{
			var posts = context.Posts.Where(x => id.Contains(x.Id));

			try
			{
				if (action == BulkActionsIndex.Delete)
				{
					await posts.ForEachAsync(item =>
					{
						item.Deleted = true;
					});
					await context.SaveChangesAsync();
					this.AddSessionError(ResponseStatus.Success, "Delete Successful");
				}
			}
			catch (Exception ex)
			{
				this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> TrashBulkAction(BulkActionsTrash action, int[] id)
		{
			var posts = context.Posts.Where(x => id.Contains(x.Id));

			try
			{
				if (action == BulkActionsTrash.Restore)
				{
					await posts.ForEachAsync(item =>
					{
						item.Deleted = false;
					});
					await context.SaveChangesAsync();
					this.AddSessionError(ResponseStatus.Success, "Restore Successful");
				}
			}
			catch (Exception ex)
			{
				this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
			}

			return RedirectToAction(nameof(Trash));
		}
	}
}