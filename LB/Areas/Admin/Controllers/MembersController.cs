using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Root, Manager")]
    public class MembersController : Controller
    {
        private readonly Context context;
        public const int COUNT = 20;

        public MembersController(Context context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? p, int? s, string search)
        {
            var members = context.Members
                .Include(x => x.AppUser)
                .Include(x => x.Country)
                .Include(x => x.Groups)
                .ThenInclude(x => x.DiscipleshipGroup)
                .Include(x => x.ResourceDownloads)
                .ThenInclude(x => x.Resource)
                .OrderBy(x => x.Created)
                .AsQueryable();

            if (search != null)
            {
                members = members.Where(x => x.AppUser.FullName.Match(search));
                ViewData["search"] = search;
            }

            //return Json(members);

            return View(await PaginatedList<Member>.CreateAsync(members, p ?? 1, s ?? COUNT));
        }

        [ActionName("Resources")]
        public async Task<IActionResult> _MemberResources(Guid id)
        {
            var resources = await context.ResourceDownloads
                .Include(x => x.Resource)
                .Where(x => x.MemberId == id)
                .ToListAsync();

            return PartialView("_MemberResources", resources);
        }

        [ActionName("Discipleship")]
        public async Task<IActionResult> _DiscipleshipGroups(Guid id)
        {
            var groups = await context.DiscipleshipMembers
                .Include(x => x.DiscipleshipGroup)
                .Where(x => x.MemberId == id)
                .ToListAsync();
            return PartialView("_DiscipleshipGroups", groups);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Countries = await context.Countries.ToListAsync();
            ViewBag.ResourceAccesses = await context.ResourceAccesses.ToListAsync();
            var member = await context.Members
                .Include(x => x.AppUser)
                .Include(x => x.Country)
                .Include(x => x.ResourceAccesses)
                .FirstOrDefaultAsync(x => x.AppUserId == id);

            return View(member);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(Guid id)
        {
            ViewBag.Countries = await context.Countries.ToListAsync();
            ViewBag.ResourceAccesses = await context.ResourceAccesses.ToListAsync();
            var member = await context.Members
                .Include(x => x.AppUser)
                .Include(x => x.Country)
                .Include(x => x.ResourceAccesses)
                .FirstOrDefaultAsync(x => x.AppUserId == id);

            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync(member))
                {
                    member.ResourceAccesses = new List<ResourceAccessMember>();
                    if (member.SelectedResourceAccess != null && member.SelectedResourceAccess.Length > 0)
                    {
                        foreach (var access in member.SelectedResourceAccess)
                        {
                            member.ResourceAccesses.Add(new ResourceAccessMember
                            {
                                ResourceAccessId = int.Parse(access)
                            });
                        }
                    }

                    try
                    {
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("error", ex.Message);
                    }
                }
            }

            return View(member);
        }



        public JsonResult ResourceDownloads(Guid member)
        {
            var downloads = context.ResourceDownloads.Include(x => x.Resource).Where(x => x.MemberId == member);
            return Json(downloads);
        }
    }
}