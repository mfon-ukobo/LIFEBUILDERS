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
    public class SiteGalleryController : Controller
    {
        private readonly Context context;

        public SiteGalleryController(Context context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var galleryItems = context.Gallery;
            return View(await PaginatedList<GalleryItem>.CreateAsync(galleryItems, 1, 12));
        }
        public JsonResult PictureList()
        {
            return Json(context.Gallery.Include(x => x.Image).OrderByDescending(x => x.Id).ToList());
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GalleryItem galleryItem)
        {
            if (ModelState.IsValid)
            {
                galleryItem.Modified = galleryItem.Created = DateTime.Now;
                try
                {
                    context.Gallery.Add(galleryItem);
                    await context.SaveChangesAsync();
                    return Json(new { Success = true, Message = "Operation Successful" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message });
                }
            }
            return Json(new { Success = false, Message = "Operation Unsuccessful" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var galleryItem = await context.Gallery.Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);
            return PartialView(galleryItem);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var galleryItem = await context.Gallery.Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);
            int oldImage = galleryItem.ImageId;
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync(galleryItem))
                {
                    var image = context.Gallery.Where(x => x.ImageId == galleryItem.ImageId);
                    if (image.Count() > 0 && galleryItem.ImageId != oldImage)
                    {
                        return Json(new { Success = false, Message = $"This Image has already been used. {galleryItem.ImageId}" });
                    }
                    galleryItem.Modified = DateTime.Now;
                    try
                    {
                        await context.SaveChangesAsync();
                        return Json(new { Success = true, Message = "Operation Successful" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, Message = ex.Message });
                    }
                }
            }
            return Json(new { Success = false, Message = "Operation Unsuccessful" });
        }
    }
}