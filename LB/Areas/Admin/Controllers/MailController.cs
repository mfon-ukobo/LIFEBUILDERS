using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MailController : Controller
    {
        private readonly Context context;
        private readonly IEmailService emailService;

        public MailController(Context context, IEmailService emailService)
        {
            this.context = context;
            this.emailService = emailService;
        }

        public async Task<IActionResult> Index(int? p, int? s)
        {
            var emails = emailService.GetEmails().OrderByDescending(x => x.Date);
            return View(PaginatedList<MimeMessage>.Create(emails, p ?? 1, 10));
        }
    }
}