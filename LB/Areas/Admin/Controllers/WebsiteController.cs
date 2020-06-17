using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using LB.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Root, Manager, Editor")]
    public class WebsiteController : Controller
    {
        private readonly Context context;

        public WebsiteController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //--------------------------------- FRONTPAGE CAROUSEL ------------------------------------
        public async Task<IActionResult> FrontPageCarousel(int? p, int? id)
        {
            return View(await FrontPageCarouselModel(p, id));
        }

        [GetErrors]
        public async Task<FrontPageCarouselView> FrontPageCarouselModel(int? p = null, int? id = null)
        {
            var model = new FrontPageCarouselView()
            {
                FrontPageCarousels = await PaginatedList<FrontPageCarousel>.CreateAsync(context.FrontPageCarousels.Include(x => x.Image), p ?? 1, 10),
                IsEditing = id.HasValue,
                FrontPageCarousel = id.HasValue ? await context.FrontPageCarousels.FirstOrDefaultAsync(x => x.Id == id) : null
            };

            return model;
        }

        [HttpPost]
        [GetErrors]
        public async Task<IActionResult> AddCarousel(FrontPageCarousel carousel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    carousel.Created = carousel.Modified = DateTime.Now;
                    context.FrontPageCarousels.Add(carousel);
                    await context.SaveChangesAsync();
                    this.AddSessionError(ResponseStatus.Success, "Carousel Added");
                    return RedirectToAction(nameof(FrontPageCarousel));
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, $"Error: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            return View(nameof(FrontPageCarousel), await FrontPageCarouselModel());
        }

        [HttpPost]
        [GetErrors]
        public async Task<IActionResult> EditCarousel(int? id, int? p)
        {
            var carousel = await context.FrontPageCarousels.FirstOrDefaultAsync(x => x.Id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync(carousel))
                    {
                        carousel.Modified = DateTime.Now;
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(FrontPageCarousel), new { p });
                    }
                }
                catch (Exception ex)
                {
                    this.AddSessionError(ResponseStatus.Error, $"Error: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            return View(nameof(FrontPageCarousel), await FrontPageCarouselModel(p, id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCarousel(int? p, int? id)
        {
            var carousel = await context.FrontPageCarousels.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                context.FrontPageCarousels.Remove(carousel);
                await context.SaveChangesAsync();
                this.AddSessionError(ResponseStatus.Success, "Carousel Deleted");
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Error, $"Carousel not Deleted: {ex.InnerException?.Message ?? ex.Message}");
            }
            return RedirectToAction(nameof(FrontPageCarousel), new { p });
        }
    }
}