using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LB.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using LB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using LB.ViewModels;
using System.Text;
using LB.Models.ViewModels;

namespace LB.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration config;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IEmailService emailService;
        private readonly NotificationManager notificationMgr;
        private readonly string appEmail = "info@lifebuildersresource.org";
        private readonly string appEmailPassword = "Fl0@c0v3r";
        private readonly string RoleName = "Member";

        public AccountController(Context context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailSender emailSender, RoleManager<IdentityRole<Guid>> roleManager, IEmailService emailService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.notificationMgr = new NotificationManager(context);
        }


        /// <summary>
        /// Login Get
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            if (HttpContext.Request.Query.ContainsKey("ReturnUrl"))
            {
                ViewBag.Error = "You must be logged in to view this page";
            }
            return View();
        }


        /// <summary>
        /// Login Post
        /// </summary>
        /// <param name="login"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);
            if (result.Succeeded)
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                return LocalRedirect(returnUrl);
            }
            else
            {
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("NotAllowed", "You are not allowed to login yet. Confirm your email!");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Login credentials invalid!");
                }
            }

            return View(login);
        }


        /// <summary>
        /// Register Member
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register(bool? discipleship)
        {
            if (await roleManager.FindByNameAsync("Member") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = "Member"
                });
            }
            ViewBag.Countries = context.Countries.ToList();
            var model = (discipleship.HasValue && discipleship.Value) ? new AppUser { IsDiscipleshipRegistration = true } : null;
            return View(model);
        }


        /// <summary>
        /// Register New Member
        /// </summary>
        /// <param name="user"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [GetErrors]
        public async Task<IActionResult> Register(AppUser user, string returnUrl = null)
        {
            ViewBag.Countries = context.Countries.ToList();
            returnUrl = returnUrl ?? Url.Content("~/");

            if (!user.IsPassword) // if the passwords are not the same
            {
                ModelState.AddModelError(nameof(user.Password), "Passwords Do Not Match");
                return View();
            }

            if (ModelState.IsValid)
            {
                string password = user.Password;
                user.Member.Created = user.Member.Modified = DateTime.Now;
                user.UserName = user.Email;
                user.EmailConfirmed = true; // remember to remove     ////////////////////////////
                var result = await userManager.CreateAsync(user, password); // add user to database

                if (result.Succeeded)
                {
                    // Add Notification Log
                    await notificationMgr.CreateNotification($"New Member <b>{user.Email}</b> created", Url.Action("Index", "Members", new { area = "Admin" }));

                    // Add User to Role 'Member'
                    var roleResult = await userManager.AddToRoleAsync(user, RoleName);
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        return View("Error");
                    }

                    // Generate Email Message Variable
                    var message = new StringBuilder();

                    // Generate email confirmation token
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Generate callback url for email confirmation
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, Request.Scheme, "www.lifebuildersresource.org");

                    // create message
                    message.Append($"Please confirm your account by <a target='_blank' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // send mail
                    /*var emailResult = await emailSender.SendEmailAsync(appEmail, user.Email, "Confirm your email",
                    message.ToString(), appEmail, appEmailPassword);*/

                    try
                    {
                        EmailMessage emailMessage = new EmailMessage
                        {
                            Subject = "Account Registration",
                            Content = message.ToString(),
                            FromAddresses = new List<EmailAddress>() { new EmailAddress { Address = "noreply@lifebuildersresource.org", Name = "Lifebuilders Resource" } },
                            ToAddresses = new List<EmailAddress>() { new EmailAddress { Address = user.Email, Name = user.FullName } }
                        };

                        var emailResult = await emailService.SendAsync(emailMessage);
                        if (emailResult.Status)
                        {
                            this.AddSessionError(ResponseStatus.Success, "Confirmation Mail Sent");
                        }
                        else
                        {
                            this.AddSessionError(ResponseStatus.Error, "Error Sending Mail. Please try again later. If problem persists, send us a mail at support@lifebuildersresource.org");
                        }
                    }
                    catch
                    {
                        this.AddSessionError(ResponseStatus.Error, "Error Sending Mail. Please try again later. If problem persists, send us a mail at support@lifebuildersresource.org");
                    }

                    // check if is a discipleship registration
                    if (user.IsDiscipleshipRegistration)
                    {
                        // route to discipleship registration for members now that user is registered
                        return RedirectToAction("Member", "Discipleship", new DiscipleshipMemberRegistration { Email = user.Email });
                    }
                    else
                    {
                        return View("RegistrationSuccess");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            return View(user);
        }

        /// <summary>
        /// Confirm Email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);

            // Confirm email address
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, true);
                return LocalRedirect(Url.Content("~/"));
            }

            return LocalRedirect(Url.Content("~/"));
        }


        /// <summary>
        /// Resend Confirmation Email
        /// </summary>
        /// <returns></returns>
        [GetErrors]
        public IActionResult ResendConfirmationEmail()
        {
            return View();
        }


        /// <summary>
        /// Resend Confirmation Email Post
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [GetErrors]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            // find user by email
            var user = await userManager.FindByEmailAsync(email);

            if (user != null) // user exists
            {
                var message = new StringBuilder();

                // generate email confirmation token
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                // generate callback url
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, Request.Scheme, "www.lifebuildersresource.org");

                // create message
                message.Append($"Please confirm your account by <a target='_blank' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                /*var emailResult = await emailSender.SendEmailAsync(appEmail, user.Email, "Confirm your email",
                message.ToString(), appEmail, appEmailPassword);*/

                try
                {
                    EmailMessage emailMessage = new EmailMessage
                    {
                        Subject = "Account Registration",
                        Content = message.ToString(),
                        FromAddresses = new List<EmailAddress>() { new EmailAddress { Address = "noreply@lifebuildersresource.org", Name = "Lifebuilders Resource" } },
                        ToAddresses = new List<EmailAddress>() { new EmailAddress { Address = user.Email, Name = user.FullName } }
                    };

                    var emailResult = await emailService.SendAsync(emailMessage);
                    if (emailResult.Status)
                    {
                        this.AddSessionError(ResponseStatus.Success, "Confirmation Mail Sent");
                    }
                    else
                    {
                        this.AddSessionError(ResponseStatus.Error, "Error Sending Mail. Please try again later. If problem persists, send us a mail at support@lifebuildersresource.org");
                    }
                }
                catch
                {
                    this.AddSessionError(ResponseStatus.Error, "Error Sending Mail. Please try again later. If problem persists, send us a mail at support@lifebuildersresource.org");
                }
            }

            return View("RegistrationSuccess");
        }


        /// <summary>
        /// Reset Password
        /// </summary>
        /// <returns></returns>
        public IActionResult ResetPassword() => View();
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var message = new StringBuilder();
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmPasswordReset", "Account", new { userId = user.Id, code = code }, Request.Scheme, "www.lifebuildersresource.org");

                message.Append($"Please reset your account password by <a target='_blank' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                var emailResult = await emailSender.SendEmailAsync(appEmail, user.Email, "Reset Your Password",
                message.ToString(), appEmail, appEmailPassword);
                ViewBag.Message = "A password reset mail has been sent to your email address.";
            }
            else
            {
                ViewBag.Message = "No User with that Email!";
            }

            return View("PasswordResetSend");
        }


        /// <summary>
        /// Confirm Password Reset
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConfirmPasswordReset(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var view = new PasswordResetView
                {
                    Code = code,
                    UserId = userId
                };
                return View(view);
            }

            return LocalRedirect(Url.Content("~/"));
        }


        /// <summary>
        /// Confirm Password Reset Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmPasswordReset(PasswordResetView model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            
            if (!model.IsPassword)
            {
                ModelState.AddModelError("Password", "Passwords do not match");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("PasswordResetSuccessful");
            }

            ModelState.AddModelError("", "Password Reset Not Successful");
            return View(model);
        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}