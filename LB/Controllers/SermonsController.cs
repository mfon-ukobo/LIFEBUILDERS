using LB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Controllers
{
	public class SermonsController : Controller
	{
		private readonly Context context;

		public SermonsController(Context context)
		{
			this.context = context;
		}

		public async Task<IActionResult> Index(int? page, int? size)
		{
			var sermons = await PaginatedList<Sermon>.CreateAsync(context.Sermons.Include(x => x.SiteImage), page ?? 1, size ?? 10);
			return View(sermons);
		}

		public async Task<IActionResult> Sermon(int id)
		{
			var sermon = await context.Sermons
				.Include(x => x.SiteImage)
				.FirstOrDefaultAsync(x => x.Id == id);
			return View(sermon);
		}
	}
}
