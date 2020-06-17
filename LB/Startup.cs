﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LB.Models;
using LB.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LB
{
	public class Startup
	{
		public const string CookieScheme = "LifebuildersResource";
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDistributedMemoryCache();

			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromSeconds(2);
				options.Cookie.HttpOnly = true;
				// Make the session cookie essential
				options.Cookie.IsEssential = true;
			});

			services.AddRouting(setupAction => {
				setupAction.LowercaseUrls = true;
			});


			services.AddDbContext<Context>(options =>
				options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedEmail = true;
			}).AddEntityFrameworkStores<Context>()
			.AddDefaultTokenProviders();

			services.Configure<DataProtectionTokenProviderOptions>(o =>
				o.TokenLifespan = TimeSpan.FromHours(3));

			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "Lifebuilders_cookie";
				options.Cookie.SameSite = SameSiteMode.None;
				options.LoginPath = new PathString("/admin/accounts/login");
				options.AccessDeniedPath = new PathString("/admin/accounts/accessdenied");
				options.ExpireTimeSpan = TimeSpan.FromDays(5);
				options.SlidingExpiration = true;
				options.Events = new CookieAuthenticationEvents
				{
					OnRedirectToLogin = ctx =>
					{
						var requestPath = ctx.Request.Path;
						if (requestPath.Value.StartsWith("/admin"))
						{
							ctx.Response.Redirect($"/admin/accounts/login?ReturnUrl={WebUtility.UrlEncode(requestPath)}");
						}
						else
						{
							ctx.Response.Redirect($"/account/login?ReturnUrl={WebUtility.UrlEncode(requestPath)}");
						}

						return Task.CompletedTask;
					},
					OnRedirectToAccessDenied = ctx =>
					{
						var requestPath = ctx.Request.Path;
						if (requestPath.Value.StartsWith("/admin"))
						{
							ctx.Response.Redirect($"/admin/accounts/login?ReturnUrl={WebUtility.UrlEncode(requestPath)}");
						}
						else
						{
							ctx.Response.Redirect($"/account/login?ReturnUrl={WebUtility.UrlEncode(requestPath)}");
						}

						return Task.CompletedTask;
					}
				};
			});

			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
			services.AddSingleton<Services.IEmailSender, EmailSender>();

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddJsonOptions(
					options =>
					{
						options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
					}
				);

			services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
			services.AddTransient<IEmailService, EmailService>();

			//services.ConfigureApplicationCookie(options =>
			//{
			//	options.Cookie.Name = CookieScheme;
			//	options.Cookie.SameSite = SameSiteMode.None;
			//	options.LoginPath = new PathString("/admin/accounts/login");
			//	options.AccessDeniedPath = new PathString("/admin/accessdenied");
			//});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//if (env.IsDevelopment())
			//{
			//	app.UseDeveloperExceptionPage();
			//}
			//else
			//{
			//	app.UseExceptionHandler("/Home/Error");
			//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			//	app.UseHsts();
			//}

			app.UseDeveloperExceptionPage();

			app.UseSession();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute(
				  name: "areas",
				  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);
			});

			
		}
	}
}
