using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using LB.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LB.Controllers
{
    public class DiscipleshipController : Controller
    {
        private readonly Context context;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly NotificationManager notificationManager;

        public DiscipleshipController(Context context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailService = emailService;
            this.notificationManager = new NotificationManager(context);
        }

        [GetErrors]
        public IActionResult Index()
        {
            ViewBag.Countries = new SelectList(context.Countries, "Id", "Name");
            return View();
        }

        [Route("/discipleship/registration/nonmember")]
        [GetErrors]
        public IActionResult NonMember()
        {
            ViewBag.Countries = new SelectList(context.Countries, "Id", "Name");
            return View();
        }

        [Route("/discipleship/registration/member")]
        [GetErrors]
        public IActionResult Member(DiscipleshipMemberRegistration model = null)
        {
            return View(model);
        }


        [Route("/discipleship/registration/member")]
        [GetErrors]
        [HttpPost]
        [ActionName(nameof(Member))]
        public async Task<IActionResult> MemberPost(/*string email, DiscipleshipType? type*/ DiscipleshipMemberRegistration memberReg)
        {
            ViewBag.Countries = new SelectList(context.Countries, "Id", "Name");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(memberReg.Email))
                {
                    if (memberReg.Type == null)
                    {
                        this.AddSessionError(ResponseStatus.Error, "Please select a discipleship type.");
                        return View(memberReg);
                    }

                    var user = await userManager.Users.Include(x => x.Member).FirstOrDefaultAsync(x => x.NormalizedUserName == memberReg.Email.ToUpper());
                    memberReg.Type = memberReg.Type ?? DiscipleshipType.Online;

                    if (user != null)
                    {
                        var reg = new DiscipleshipRegistration
                        {
                            Member = user.Member,
                            Created = DateTime.Now,
                            Type = memberReg.Type.Value
                        };


                        try
                        {
                            // Database operation
                            context.DiscipleshipRegistrations.Add(reg);
                            await context.SaveChangesAsync();


                            // mail content
                            string message = $"You have successfully registered for <b>{reg.Type.ToString().ToUpper()}</b> discipleship. We will get back to you shortly.";

                            // create email message
                            EmailMessage emailMessage = new EmailMessage
                            {
                                Subject = "Discipleship Registration",
                                Content = message,
                                FromAddresses = new List<EmailAddress>() { new EmailAddress { Address = "noreply@lifebuildersresource.org", Name = "Lifebuilders Resource Discipleship" } },
                                ToAddresses = new List<EmailAddress>() { new EmailAddress { Address = user.Email, Name = user.FullName } }
                            };

                            var emailResult = await emailService.SendAsync(emailMessage);

                            // Success message
                            this.AddSessionError(ResponseStatus.Success, "Discipleship Registration successful. You will not be contacted if your email is not confirmed. Please do not register more than once.");

                            // Add Admin notification
                            await notificationManager.CreateNotification($"New Discipleship Registration <b>{user.Email}</b> created", Url.Action("Index", "Members", new { area = "Admin" }));

                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception)
                        {
                            this.AddSessionError(ResponseStatus.Error, "Could not register for discipleship please try again later.");
                        }

                    }
                    else
                    {
                        this.AddSessionError(ResponseStatus.Error, "You have to be a member to use this method! Register and try again or use the non-member registration page.");
                    }
                }
            }

            return View(memberReg);
        }
    }
}