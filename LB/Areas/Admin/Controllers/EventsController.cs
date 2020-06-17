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
    [Authorize(Roles = "Root, Editor, Manager")]
    public class EventsController : Controller
    {
        private readonly Context context;
        public const int COUNT = 20;

        public EventsController(Context context)
        {
            this.context = context;
        }

        [GetErrors]
        public async Task<IActionResult> Index(int? p, int? s, string search)
        {
            var events = EventList(search);
            return View(await PaginatedList<LBEvent>.CreateAsync(events, p ?? 1, s ?? COUNT));
        }

        [GetErrors]
        public async Task<IActionResult> Trash(int? p, int? s, string search)
        {
            var events = EventList(search, true);
            return View(await PaginatedList<LBEvent>.CreateAsync(events, p ?? 1, s ?? COUNT));
        }

        public IQueryable<LBEvent> EventList(string search = null, bool trash = false)
        {
            var events = context.Events.Include(x => x.Image).AsQueryable();

            events = events.Where(x => x.Deleted == trash);

            if (search != null)
            {
                events = events.Where(x => x.Title.Match(search));
                ViewData["search"] = search;
            }

            return events;
        }

        public IActionResult Create()
        {
            return View();
        }

        [GetErrors]
        [HttpPost]
        public async Task<IActionResult> Create(LBEvent @event)
        {
            if (ModelState.IsValid)
            {
                @event.Created = @event.Modified = DateTime.Now;

                try
                {
                    context.Events.Add(@event);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Event Added Successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(@event);
        }

        [GetErrors]
        public async Task<IActionResult> Edit(string slug)
        {
            var model = await context.Events.Include(x => x.Image).FirstOrDefaultAsync(x => x.Slug == slug);
            return View(model);
        }

        [GetErrors]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var model = await context.Events.Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);

            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync(model))
                {
                    model.Modified = DateTime.Now;
                    try
                    {
                        string slug = model.Slug;
                        await context.SaveChangesAsync();
                        this.AddSessionError(ResponseStatus.Success, "Update Successful");
                        return RedirectToAction(nameof(Edit), new { slug = slug });
                    }
                    catch (Exception ex)
                    {
                        this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                model.Deleted = true;
                await context.SaveChangesAsync();
                this.AddSessionError(ResponseStatus.Success, "Delete Successful");
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var model = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                model.Deleted = false;
                await context.SaveChangesAsync();
                this.AddSessionError(ResponseStatus.Success, "Restore Successful");
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Trash));
        }

        [HttpPost]
        public async Task<IActionResult> IndexBulkAction(BulkActionsIndex action, int[] id)
        {
            var posts = context.Events.Where(x => id.Contains(x.Id));

            try
            {
                if (action == BulkActionsIndex.Delete)
                {
                    await posts.ForEachAsync(item =>
                    {
                        item.Deleted = true;
                    });
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Delete Successful");
                }
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> TrashBulkAction(BulkActionsTrash action, int[] id)
        {
            var posts = context.Events.Where(x => id.Contains(x.Id));

            try
            {
                if (action == BulkActionsTrash.Restore)
                {
                    await posts.ForEachAsync(item =>
                    {
                        item.Deleted = false;
                    });
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Restore Successful");
                }
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Trash));
        }
    }
}