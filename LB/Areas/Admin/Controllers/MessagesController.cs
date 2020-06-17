using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Root, Manager")]
    public class MessagesController : Controller
    {
        private readonly Context context;

        public MessagesController(Context context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? p, int? s)
        {
            var messages = context.Messages.OrderByDescending(x => x.Id);
            return View(await PaginatedList<Message>.CreateAsync(messages, p ?? 1, s ?? 10));
        }
    }
}