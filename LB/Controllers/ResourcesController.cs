using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using LB.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IOFile = System.IO.File;

namespace LB.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly Context context;
        private readonly UserManager<AppUser> userManager;
        private readonly IHostingEnvironment env;

        public ResourcesController (Context context, UserManager<AppUser> userManager, IHostingEnvironment env)
        {
            this.context = context;
            this.userManager = userManager;
            this.env = env;
        }


        /// <summary>
        /// Resource grouped by category
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? page)
        {
            //var model = context.ResourceCategories.Include(x => x.Resources).ThenInclude(x => x.Image);
            var model = new ResourceView
            {
                ResourceCategories = await PaginatedList<ResourceCategory>.CreateAsync(context.ResourceCategories.OrderByDescending(x => x.Id), page??1, 5),
                Resources = await context.Resources.Include(x => x.Image).OrderByDescending(x => x.Id).ToListAsync()
            };

            return View(model);
            //return Json(await PaginatedList<ResourceCategory>.CreateAsync(model, page ?? 1, 5));
        }


        /// <summary>
        /// Resource Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> Category(string id, int? page)
        {
            //var model = context.ResourceCategories.Include(x => x.Resources).ThenInclude(x => x.Image);
            var userId = Guid.Parse(userManager.GetUserId(User));
            var userResourceAccesses = context.ResourceAccesses.Where(x => x.Members.Select(y => y.MemberId).Contains(userId));
            var resources = context.Resources.Include(x => x.Image).Include(x => x.Category).Include(x => x.ResourceAccesses).ThenInclude(x => x.ResourceAccess).Where(x => x.Category.Slug == id);

            //return View(resources);
            return View(await PaginatedList<Resource>.CreateAsync(resources, page ?? 1, 10));
        }


        /// <summary>
        /// Open Resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GetErrors]
        public async Task<IActionResult> Resource(string id)
        {
            //var userId = Guid.Parse(userManager.GetUserId(User));
            //var member = await context.Members.Include(x => x.ResourceAccesses).FirstOrDefaultAsync(x => x.AppUserId == userId);
            //var admin = await context.AdminUsers.FirstOrDefaultAsync(x => x.AppUserId == userId);

            ViewBag.Resources = (await context.Resources.Include(x => x.Image).ToListAsync()).TakeLast(5);

            var resource = (await context.Resources.Include(x => x.Image).Include(x => x.Category).Include(x => x.ResourceAccesses).ThenInclude(x => x.ResourceAccess).ToListAsync()).FirstOrDefault(x => x.Slug == id);

            if (resource == null)
                return View("NotFound");

            /*if (member != null)
            {
                if (!HasResourceAccess(resource, member))
                    return View("NoAccess", new ResourceAccessError { Resource = id });
            }

            if (admin == null && member == null)
            {
                return View("NoAccess", new ResourceAccessError { Resource = id });
            }*/

            return View(resource);
        }


        /// <summary>
        /// Return Resource Downloadable File
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GetErrors]
        public async Task<IActionResult> Download(string id)
        {
            var uploadsFolder = Path.Combine(env.WebRootPath, "uploads/resources");

            var userId = Guid.Parse(userManager.GetUserId(User));
            var member = await context.Members.Include(x => x.ResourceAccesses).FirstOrDefaultAsync(x => x.AppUserId == userId);
            var admin = await context.AdminUsers.FirstOrDefaultAsync(x => x.AppUserId == userId);
            var resource = await context.Resources.Include(x => x.ResourceDownloads).Include(x => x.ResourceAccesses).Where(x => x.Slug == id).FirstOrDefaultAsync();

            if (resource == null)
                return View("NotFound");

            if (member != null)
            {
                if (!HasResourceAccess(resource, member))
                    return View("NoAccess");
            }

            if (admin == null && member == null)
            {
                return View("NoAccess");
            }

            try
            {
                resource.ResourceDownloads.Add(new ResourceDownload
                {
                    Member = member,
                    Created = DateTime.Now
                });
                await context.SaveChangesAsync();

                var stream = new FileStream(Path.Combine(uploadsFolder, resource.ResourceUrl), FileMode.Open);
                return File(stream, "application/octet-stream", resource.ResourceUrl);
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, "There has been an error contact the administrator to report this issue");
                return RedirectToAction(nameof(Resource), new { id });
            }
        }

        [Route("/resources/accessrequest/{slug}")]
        public async Task<IActionResult> RequestAccess(string slug)
        {
            var resource = await context.Resources.Include(x => x.Category).Include(x => x.Image).Where(x => x.Slug == slug).FirstOrDefaultAsync();
            var member = Guid.Parse(userManager.GetUserId(User));
            var view = new ResourceAccessRequest
            {
                ResourceId = resource.Id,
                Resource = resource,
                MemberId = member
            };
            return View(view);
        }
        

        [HttpPost]
        [Route("/resources/accessrequest/{slug}")]
        public async Task<IActionResult> RequestAccess(ResourceAccessRequest request)
        {
            var resource = await context.Resources.Include(x => x.Category).Include(x => x.Image).Where(x => x.Id == request.ResourceId).FirstOrDefaultAsync();

            request.Resource = resource;

            if (!ModelState.IsValid)
                return View(request);

            try
            {
                context.ResourceAccessRequests.Add(request);
                await context.SaveChangesAsync();
                this.AddSessionError(ResponseStatus.Success, "Your request has been submitted");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                this.AddSessionError(ResponseStatus.Error, "Error processing this request. Try again later or contact the administrator if this problem persists.");
                return View(request);
            }
        }



        public bool HasResourceAccess(Resource resource, Member member)
        {
            if (resource.ResourceAccesses.Any())
            {
                foreach (var access in resource.ResourceAccesses)
                {
                    if (member.ResourceAccesses.Select(x => x.ResourceAccessId).Contains(access.ResourceAccessId))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}