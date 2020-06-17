using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Root, Manager")]
    public class DiscipleshipController : Controller
    {
        private readonly Context context;
        private readonly UserManager<AppUser> userManager;
        public const int COUNT = 20;

        public DiscipleshipController (Context context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [GetErrors]
        public async Task<IActionResult> Index(int? p, int? s, string search)
        {
            var groups = GroupList(search);
            return View(await PaginatedList<DiscipleshipGroup>.CreateAsync(groups, p ?? 1, s ?? COUNT));
        }

        [GetErrors]
        public async Task<IActionResult> Trash(int? p, int? s, string search)
        {
            var groups = GroupList(search, true);
            return View(await PaginatedList<DiscipleshipGroup>.CreateAsync(groups, p ?? 1, s ?? COUNT));
        }

        [NonAction]
        public IQueryable<DiscipleshipGroup> GroupList(string search = null, bool trash = false)
        {
            var groups = context.DiscipleshipGroups
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.AppUser)
                .Where(x => x.Deleted == trash)
                .OrderByDescending(x => x.Id)
                .Where(x => x.Deleted == trash)
                .AsQueryable();

            if (search != null)
            {
                groups.Where(x => x.Name.Match(search));
                ViewData["search"] = search;
            }

            return groups;
        }

        public async Task<IActionResult> GroupMembers(int id)
        {
            var group = await context.DiscipleshipGroups
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            return PartialView(group.Members);
        }

        public IActionResult Create()
        {
            return View();
        }

        public JsonResult GetMembers(string[] id)
        {
            var members = userManager.Users
                .Where(x => id.Contains(x.Id.ToString()))
                .Select(x => new { x.Id, x.FullName, x.Email });
            return Json(members);
        }

        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> Create(DiscipleshipGroup group, Guid[] selectedMembers)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    group.Created = DateTime.Now;

                    foreach (var member in selectedMembers)
                    {
                        group.Members.Add(new DiscipleshipMember
                        {
                            MemberId = member
                        });
                    }

                    context.DiscipleshipGroups.Add(group);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Discipleship Group Created Successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(group);
        }

        [GetErrors]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction(nameof(Index));

            var group = await context.DiscipleshipGroups.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id);
            return View(group);
        }

        public JsonResult GetGroupMembers(int id)
        {
            var members = context.DiscipleshipMembers.Where(x => x.DiscipleshipGroupId == id).Select(x => x.MemberId);
            //HashSet<string> members = new HashSet<string>(group.Select(y => y.))
            return Json(members);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        [GetErrors]
        public async Task<IActionResult> EditPost(int? id, Guid[] selectedMembers)
        {
            if (!id.HasValue)
                return RedirectToAction(nameof(Index));

            var group = await context.DiscipleshipGroups.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync(group))
                    {
                        group.Members = new List<DiscipleshipMember>();
                        foreach (var member in selectedMembers)
                        {
                            group.Members.Add(new DiscipleshipMember
                            {
                                MemberId = member
                            });
                        }

                        await context.SaveChangesAsync();
                        this.AddSessionError(ResponseStatus.Success, "Discipleship Group Update Successfully");
                        return RedirectToAction(nameof(Edit), new { id });
                    }
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }

            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var group = await context.DiscipleshipGroups.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id);
                    group.Deleted = true;
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Delete Successful");
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                this.AddSessionError(ResponseStatus.Error, "No item selected");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var group = await context.DiscipleshipGroups.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id);
                    group.Deleted = false;
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Restore Successful");
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                this.AddSessionError(ResponseStatus.Error, "No item selected");
            }

            return RedirectToAction(nameof(Trash));
        }

        [HttpPost]
        public async Task<IActionResult> PremanentDelete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var group = await context.DiscipleshipGroups.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id);
                    context.DiscipleshipGroups.Remove(group);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Permament Delete Successful");
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                this.AddSessionError(ResponseStatus.Error, "No item selected");
            }

            return RedirectToAction(nameof(Trash));
        }


        //----------------------------------------- REQUESTS --------------------------------

        public async Task<IActionResult> Requests(int? p, int? s, string search)
        {
            var requests = context.DiscipleshipRegistrations.Include(x => x.Member).ThenInclude(x => x.AppUser).AsQueryable();

            if (search != null)
            {
                requests = requests.Where(x => x.Member.AppUser.FullName.Match(search));
                ViewData["search"] = search;
            }

            return View(await PaginatedList<DiscipleshipRegistration>.CreateAsync(requests, p ?? 1, s ?? COUNT));
        }

        [HttpPost]
        public async Task<JsonResult> ToggleComplete(int id)
        {
            var request = await context.DiscipleshipRegistrations.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                request.Completed = !request.Completed;
                await context.SaveChangesAsync();
                return Json(new { status = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}