using LB.Models;
using IOFile = System.IO.File; /**/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace LB.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]
	[Authorize(Roles = "Admin")]
	public class SermonsController : Controller
	{
		private readonly Context context;
		private readonly IWebHostEnvironment env;
		private readonly string folderURL;

		public SermonsController(Context context,
			IWebHostEnvironment env, IConfiguration config)
		{
			this.context = context;
			this.env = env;
			folderURL = config.GetValue<string>("BaseURL") + "/uploads/sermons/";
		}

		// GET: SermonsController
		public async Task<ActionResult> Index(int? pageIndex, int? pageSize)
		{
			var sermons = await PaginatedList<Sermon>.CreateAsync(context.Sermons, pageIndex ?? 1, pageSize ?? 20);
			return View(sermons);
		}

		// GET: SermonsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: SermonsController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: SermonsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Sermon sermon, IFormFile file)
		{
			if (!ModelState.IsValid)
			{
				return ValidationProblem();
			}

			sermon.Slug = sermon.Title.Replace(" ", "-")
				.Replace(".", null);

			sermon.DateCreated = DateTime.Now;
			sermon.LastModified = DateTime.Now;

			try
			{
				if (file != null && file.Length > 0)
				{
					// upload folder path
					string uploadFolder = Path.Combine(env.WebRootPath, "uploads/sermons");

					// create upload folder if not exists
					if (!Directory.Exists(uploadFolder))
					{
						Utils.CreateDirectory(uploadFolder);
					}

					var ext = Path.GetExtension(file.FileName);

					// generate a random file name for the resource
					string fileName = Path.GetRandomFileName().CleanString() + ext;

					// generate file path
					string filePath = Path.Combine(uploadFolder, fileName);

					//string fileUrl = config.GetValue<string>("UploadFolders:Resources") + fileName;
					using (var stream = IOFile.Create(filePath))
					{
						await file.CopyToAsync(stream);
						sermon.FileURL = folderURL + fileName;
					}

					await context.AddAsync(sermon);
					await context.SaveChangesAsync();
				}

				return Ok(true);
			}
			catch
			{
				return StatusCode(500);
			}
		}

		// GET: SermonsController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: SermonsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: SermonsController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: SermonsController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
