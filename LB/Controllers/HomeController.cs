using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LB.Models;
using Microsoft.EntityFrameworkCore;
using LB.Models.ViewModels;
using LB.Services;

namespace LB.Controllers
{
	public class HomeController : Controller
	{
		private readonly Context context;
		private readonly IEmailService emailService;

		public HomeController(Context _context, IEmailService emailService)
		{
			context = _context;
			this.emailService = emailService;
		}

		public async Task<IActionResult> Index()
		{
			var events = await context.Events.ToListAsync();
			var model = new HomePageView
			{
				Posts = await context.Posts.Include(x => x.Image).OrderByDescending(x => x.Id).Where(x => x.Deleted == false).Take(3).ToListAsync(),
				Events = await context.Events.OrderByDescending(x => x.StartDate).Where(x => x.Deleted == false).Take(5).ToListAsync(),
				Gallery = await context.Gallery.Include(x => x.Image).OrderByDescending(x => x.Id).Take(8).ToListAsync(),
				Resources = await context.Resources.Include(x => x.Image).OrderByDescending(x => x.Id).Where(x => x.Deleted == false).Take(2).ToListAsync(),
				NextEvent = events.Where(x => DateTime.Parse(x.StartDate) > DateTime.Now && x.Deleted == false).OrderBy(x => DateTime.Parse(x.StartDate)).FirstOrDefault(),
				Carousels = await context.FrontPageCarousels.Include(x => x.Image).ToListAsync(),
				Sermons = context.Sermons.Include(x => x.SiteImage).OrderByDescending(x => x.LastModified).Take(5)
			};
			return View(model);
		}

		[Route("/About")]
		public IActionResult About()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[Route("/contact")]
		[GetErrors]
		public IActionResult Contact()
		{
			return View();
		}

		[Route("/contact")]
		[HttpPost]
		[GetErrors]
		public async Task<IActionResult> Contact(Message message)
		{
			/*try
			{
				message.Created = DateTime.Now;
				context.Messages.Add(message);
				await context.SaveChangesAsync();
				this.AddSessionError(ResponseStatus.Success, "Your message has been sent. We will get back to you shortly.");
				return RedirectToAction(nameof(Contact));
			}
			catch (Exception)
			{
				this.AddSessionError(ResponseStatus.Error, "Your message could not be sent. Please try again later.");
			}*/

			var from = new List<EmailAddress>();
			from.Add(new EmailAddress
			{
				Address = message.Email,
				Name = message.Name
			});

			var to = new List<EmailAddress>();
			to.Add(new EmailAddress
			{
				Address = "info@lifebuildersresource.org",
				Name = "Lifebuilders Resource"
			});

			try
			{
				var email = new EmailMessage
				{
					FromAddresses = from,
					ToAddresses = to,
					Content = message.Body,
					Subject = $"Message from Contact Form by {message.Name}"
				};
				await emailService.SendAsync(email);
				this.AddSessionError(ResponseStatus.Success, "Your message has been sent. We will get back to you shortly.");
				return RedirectToAction(nameof(Contact));
			}
			catch (Exception ex)
			{
				this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
			}

			return View(message);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<IActionResult> _Sidebar()
		{
			ViewBag.Posts = await context.Posts.Include(x => x.Image).OrderByDescending(x => x.Id).Take(5).ToListAsync();
			ViewBag.Tags = await context.Tags.ToListAsync();
			return PartialView();
		}
	}
}
