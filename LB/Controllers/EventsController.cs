using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LB.Controllers
{
    public class EventsController : Controller
    {
		private readonly Context context;

		public EventsController(Context _context)
		{
			context = _context;
		}

        public IActionResult Index(int? page)
        {
			var events = context.Events.Include(x => x.Image).OrderByDescending(x => x.Id).ToList();
            return View(PaginatedList<LBEvent>.Create(events, page ?? 1, 9));
        }

		[Route("/events/{id}")]
		public IActionResult Single(string id)
		{
			var ev = context.Events.Include(x => x.Image).FirstOrDefault(x => x.Slug == id);
			return View(ev);
		}
    }
}