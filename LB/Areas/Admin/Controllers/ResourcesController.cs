using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IOFile = System.IO.File; /**/
using System.Threading.Tasks;
using LB.Models;
using LB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Root, Manager, Editor")]
    public class ResourcesController : Controller
    {
        private readonly Context context;
        private readonly IHostingEnvironment env;
        private readonly IConfiguration config;
        public const int COUNT = 20;
        public const int MINCOUNT = 10;

        public ResourcesController(Context context, IHostingEnvironment env, IConfiguration config)
        {
            this.context = context;
            this.env = env;
            this.config = config;
        }

        /// <summary>
        /// Main Resources Page
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        [GetErrors]
        public async Task<IActionResult> Index(int? p, int? s, string search)
        {
            var model = context.Resources
                .Include(x => x.Image)
                .Include(x => x.ResourceAccesses)
                .ThenInclude(x => x.ResourceAccess)
                .Include(x => x.ResourceDownloads)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.AppUser)
                .OrderByDescending(x => x.Id)
                .Where(x => !x.Deleted);

            if (search != null)
            {
                model = model.Where(x => x.Title.Match(search));
                ViewData["search"] = search;
            }

            return View(await PaginatedList<Resource>.CreateAsync(model, p ?? 1, s ?? COUNT));
        }

        /// <summary>
        /// Resources Trash
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// 
        [GetErrors]
        public async Task<IActionResult> Trash(int? p, int? s, string search)
        {
            var model = context.Resources.Include(x => x.Image).OrderByDescending(x => x.Modified).Where(x => x.Deleted);

            if (search != null)
            {
                model = model.Where(x => x.Title.Match(search));
                ViewData["search"] = search;
            }

            return View(await PaginatedList<Resource>.CreateAsync(model, p ?? 1, s ?? COUNT));
        }

        /// <summary>
        /// GET: Create Resource
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.Categories = context.ResourceCategories.ToList();
            ViewBag.Accesses = context.ResourceAccesses.ToList();
            return View();
        }

        /// <summary>
        /// POST: Create Resource
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> Create(Resource resource)
        {
            ViewBag.Categories = context.ResourceCategories.ToList();
            ViewBag.Accesses = context.ResourceAccesses.ToList();

            if (resource.SelectedAccesses != null && resource.SelectedAccesses.Length > 0)
            {
                foreach (var access in resource.SelectedAccesses)
                {
                    resource.ResourceAccesses.Add(new ResourceToAccess
                    {
                        ResourceAccessId = int.Parse(access)
                    });
                }
            }


            if (resource.CategoryId == 0)
			{
                var category = await context.ResourceCategories.FirstOrDefaultAsync(x => x.Name == "Uncategorized");
                if (category == null)
				{
                    category = new ResourceCategory
                    {
                        Name = "Uncategorized",
                        Created = DateTime.Now,
                        Description = "For Uncategorized resources"
                    };

                    context.ResourceCategories.Add(category);
                    await context.SaveChangesAsync();

                    resource.CategoryId = category.Id;
				}
			}

            if (ModelState.IsValid)
            {
                resource.Modified = resource.Created = DateTime.Now;

                try
                {
                    // Check if is a local resource
                    if (resource.IsLocalResource)
                    {
                        // check if a file was uploaded
                        if (resource.LocalFile != null && resource.LocalFile.Length > 0)
                        {
                            // upload folder path
                            string uploadFolder = Path.Combine(env.WebRootPath, "uploads/resources");

                            // create upload folder if not exists
                            if (!Directory.Exists(uploadFolder))
                            {
                                Utils.CreateDirectory(uploadFolder);
                            }

                            // generate a random file name for the resource
                            string fileName = Path.GetRandomFileName().Replace(".", "") + HttpUtility.UrlEncode(resource.LocalFile.FileName.Replace(" ", ""));

                            // generate file path
                            string filePath = Path.Combine(uploadFolder, fileName);

                            //string fileUrl = config.GetValue<string>("UploadFolders:Resources") + fileName;
                            using (var stream = IOFile.Create(filePath))
                            {
                                await resource.LocalFile.CopyToAsync(stream);
                                resource.ResourceUrl = fileName;
                            }
                        }
                    }

                    context.Resources.Add(resource);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Resource created Successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }

            return View(resource);
        }

        /// <summary>
        /// GET: Edit Resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GetErrors]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = context.ResourceCategories.ToList();
            ViewBag.Accesses = context.ResourceAccesses.ToList();
            var resource = await context.Resources.Include(x => x.Image).Include(x => x.ResourceAccesses).FirstOrDefaultAsync(x => x.Id == id);
            return View(resource);
        }

        /// <summary>
        /// POST: Edit Resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="LocalFile"></param>
        /// <returns></returns>
        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormFile LocalFile)
        {
            var resource = await context.Resources.Include(x => x.Image).Include(x => x.ResourceAccesses).FirstOrDefaultAsync(x => x.Id == id);
            string uploadFolder = Path.Combine(env.WebRootPath, "uploads/resources");
            string prevResource = resource.ResourceUrl;
            bool prevType = resource.IsLocalResource;
            bool hasFile = false;

            if (!Directory.Exists(uploadFolder))
            {
                Utils.CreateDirectory(uploadFolder);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync(resource))
                    {
                        if (resource.SelectedAccesses != null && resource.SelectedAccesses.Length > 0)
                        {
                            resource.ResourceAccesses = new List<ResourceToAccess>();

                            foreach (var access in resource.SelectedAccesses)
                            {
                                resource.ResourceAccesses.Add(new ResourceToAccess
                                {
                                    ResourceAccessId = int.Parse(access)
                                });
                            }
                        }

                        if (resource.IsLocalResource)
                        {
                            if (LocalFile != null && LocalFile.Length > 0)
                            {
                                hasFile = true; // Set having local file Flag to true

                                string fileName = Path.GetRandomFileName().Replace(".", "") + HttpUtility.UrlEncode(resource.LocalFile.FileName.Replace(" ", ""));

                                string filePath = Path.Combine(uploadFolder, fileName);

                                using (var stream = IOFile.Create(filePath))
                                {
                                    await resource.LocalFile.CopyToAsync(stream);
                                    resource.ResourceUrl = fileName;
                                }
                            }
                        }

                        var result = await context.SaveChangesAsync();
                        if (prevType == true) // if previous type is a local resource
                        {
                            if (hasFile || resource.IsLocalResource == false)
                            {
                                string filePath = Path.Combine(uploadFolder, prevResource);
                                Utils.DeleteFile(filePath);
                            }
                        }

                        this.AddSessionError(ResponseStatus.Success, "Resource updated Successfully");
                        return RedirectToAction(nameof(Edit), new { id });
                    }
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewBag.Categories = context.ResourceCategories.ToList();
            ViewBag.Accesses = context.ResourceAccesses.ToList();
            return View(resource);
        }

        /// <summary>
        /// POST: Delete Resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var resource = await context.Resources.FindAsync(id);
                resource.Deleted = true;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: Restore Resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            try
            {
                var resource = await context.Resources.FindAsync(id);
                resource.Deleted = false;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.Message);
            }

            return RedirectToAction(nameof(Trash));
        }

        /// <summary>
        /// POST: Delete Resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [GetErrors]
        public async Task<IActionResult> DeletePermament(int? id)
        {
            string uploadFolder = Path.Combine(env.WebRootPath, "uploads/resources");
            var result = new StringBuilder();

            try
            {
                var resource = await context.Resources.Include(x => x.ResourceDownloads).FirstOrDefaultAsync(x => x.Id == id);
                context.Resources.Remove(resource);
                await context.SaveChangesAsync();

                result.Append("Resource Removed");

                if (resource.IsLocalResource)
                {
                    string path = Path.Combine(uploadFolder, resource.ResourceUrl);
                    IOFile.Delete(path);
                    result.Append("Resource File Deleted");
                }

                this.AddSessionError(ResponseStatus.Success, result.ToString());
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Trash));
            //return Content("Page");
        }








        /* 
         * --------------------------- CATEGORIES AND WHAT NOT ----------------------------- 
         */


        /// <summary>
        /// REsource Categories Main View
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [GetErrors]
        public async Task<IActionResult> Categories(int? id, int? p, int? s, string search)
        {
            var model = new ResourceCategoryView
            {
                ResourceCategories = await PaginatedList<ResourceCategory>.CreateAsync(CategoryList(search), p ?? 1, s ?? MINCOUNT),
                ResourceCategory = id.HasValue ? await context.ResourceCategories.FirstOrDefaultAsync(x => x.Id == id) : null,
                IsEditing = id.HasValue
            };

            return View(model);
        }

        [NonAction]
        public IQueryable<ResourceCategory> CategoryList(string search = null)
        {
            var categories = context.ResourceCategories.OrderByDescending(x => x.Id).AsQueryable();

            if (search != null)
            {
                categories = categories.Where(x => x.Name.Match(search));
                ViewData["search"] = search;
            }

            return categories;
        }

        /// <summary>
        /// Create Resource Category
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateCategory() => RedirectToAction(nameof(Categories));
        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(ResourceCategory category)
        {
            if (ModelState.IsValid)
            {
                category.Modified = category.Created = DateTime.Now;
                try
                {
                    context.ResourceCategories.Add(category);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, $"Create Operation Successful");
                    return RedirectToAction(nameof(Categories));
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }

            var model = new ResourceCategoryView
            {
                ResourceCategories = await PaginatedList<ResourceCategory>.CreateAsync(CategoryList(), 1, MINCOUNT),
                ResourceCategory = category,
                IsEditing = false
            };

            return View(nameof(Categories), model);
        }

        /// <summary>
        /// Edit resource Category
        /// </summary>
        /// <returns></returns>
        public IActionResult EditCategory(int? id, int? p, int? s, string search) => RedirectToAction(nameof(Categories), new { id, p, s, search });
        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, int? p, int? s, string search)
        {
            var category = await context.ResourceCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync(category))
                {
                    category.Modified = DateTime.Now;

                    try
                    {
                        await context.SaveChangesAsync();
                        this.AddSessionError(ResponseStatus.Success, $"Update Operation Successful");
                        return RedirectToAction(nameof(Categories), new { p, s, search });
                    }
                    catch (Exception ex)
                    {
                        this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }

            var model = new ResourceCategoryView
            {
                ResourceCategories = await PaginatedList<ResourceCategory>.CreateAsync(CategoryList(search), p ?? 1, s ?? MINCOUNT),
                ResourceCategory = category,
                IsEditing = true
            };

            return View(nameof(Categories), model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int[] id, int? p, int? s, string search)
        {
            try
            {
                var categories = context.ResourceCategories.Where(x => id.Contains(x.Id));
                context.ResourceCategories.RemoveRange(categories);
                this.AddSessionError(ResponseStatus.Success, $"Delete Operation Successful");
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Categories), new { p, s, search });
        }




        /* 
         * --------------------------- ACCESS AND WHAT NOT ----------------------------- 
         */


        /// <summary>
        /// Resource Access Main View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GetErrors]
        public async Task<IActionResult> Accesses(int? id, int? p, int? s, string search)
        {
            ViewBag.Members = context.Members.Include(x => x.AppUser).Select(x => x.AppUser).ToList();
            var model = new ResourceAccessView
            {
                ResourceAccesses = await PaginatedList<ResourceAccess>.CreateAsync(AccessList(search), p ?? 1, s ?? MINCOUNT),
                ResourceAccess = await context.ResourceAccesses.FirstOrDefaultAsync(x => x.Id == id),
                IsEditing = id.HasValue
            };
            return View(model);
        }

        [NonAction]
        public IQueryable<ResourceAccess> AccessList(string search = null)
        {
            var accesses = context.ResourceAccesses.Include(x => x.Members).OrderByDescending(x => x.Id).AsQueryable();

            if (search != null)
            {
                accesses = accesses.Where(x => x.Name.Match(search));
                ViewData["search"] = search;
            }

            return accesses;
        }

        /// <summary>
        /// POST: Create Resource Access
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> CreateAccess(ResourceAccess access)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    access.Created = access.Modified = DateTime.Now;

                    if (access.SelectedMembers != null && access.SelectedMembers.Length > 0)
                    {
                        foreach (var member in access.SelectedMembers[0].Split(","))
                        {
                            access.Members.Add(new ResourceAccessMember
                            {
                                MemberId = Guid.Parse(member)
                            });
                        }
                    }

                    context.ResourceAccesses.Add(access);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Create Operation Successful");
                    return RedirectToAction(nameof(Accesses));
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }

            var model = new ResourceAccessView
            {
                ResourceAccesses = await PaginatedList<ResourceAccess>.CreateAsync(AccessList(), 1, MINCOUNT),
                ResourceAccess = access,
                IsEditing = false
            };

            return View(nameof(Accesses), model);
        }

        /// <summary>
        /// POST: Edit Resource Access
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> EditAccess(int id, int? p, int? s, string search)
        {
            var access = await context.ResourceAccesses.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id);

            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync(access))
                {
                    try
                    {
                        access.Members = new List<ResourceAccessMember>();
                        if (access.SelectedMembers != null && access.SelectedMembers.Length > 0)
                        {
                            foreach (var member in access.SelectedMembers[0].Split(","))
                            {
                                access.Members.Add(new ResourceAccessMember
                                {
                                    MemberId = Guid.Parse(member)
                                });
                            }
                        }

                        access.Modified = DateTime.Now;
                        await context.SaveChangesAsync();
                        this.AddSessionError(ResponseStatus.Success, "Update Operation Successful");
                        return RedirectToAction(nameof(Accesses), new { p, s, search });
                    }
                    catch (Exception ex)
                    {
                        this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }

            var model = new ResourceAccessView
            {
                ResourceAccesses = await PaginatedList<ResourceAccess>.CreateAsync(AccessList(search), p ?? 1, s ?? MINCOUNT),
                ResourceAccess = access,
                IsEditing = true
            };

            return View(nameof(Accesses), model);
        }

        /// <summary>
        /// POST: Delete Resource Access
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAccess(int id, int? p, int? s, string search)
        {
            var access = await context.ResourceAccesses.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                context.ResourceAccesses.Remove(access);
                await context.SaveChangesAsync();
                this.AddSessionError(ResponseStatus.Success, "Delete Operation Successful");
                return RedirectToAction(nameof(Accesses));
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Accesses), new { p, s, search });
        }






        /*
         * --------------------------------------------- RESOURCE DOWNLOADS ---------------------------------------------------------
         */


        /// <summary>
        /// REsource Downloads
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public async Task<IActionResult> DownloadsLog(int? p, int? s)
        {            
            var downloads = context.ResourceDownloads.Include(x => x.Resource).Include(x => x.Member).ThenInclude(x => x.AppUser);
            return View(await PaginatedList<ResourceDownload>.CreateAsync(downloads, p ?? 1, s ?? 10));
        }


        /*------------------------------------- RESOURCE REQUESTS -------------------------------*/
        public async Task<IActionResult> ResourceRequests(int? p, int? s)
        {
            var requests = context.ResourceAccessRequests.Include(x => x.Resource).Include(x => x.Member).ThenInclude(x => x.AppUser).OrderByDescending(x => x.Created);
            return View(await PaginatedList<ResourceAccessRequest>.CreateAsync(requests, p ?? 1, s ?? 10));
        }
    }
}