using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using IOFile = System.IO.File;
using System.Linq;
using System.Threading.Tasks;
using LazZiya.ImageResize;
using LB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Root")]
    public class GalleryController : Controller
    {
        private readonly Context context;
        private readonly IHostingEnvironment env;

        public GalleryController(Context context, IHostingEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SiteImage siteImage, IFormFile file)
        {
            siteImage.Created = DateTime.Now;
            siteImage.Modified = DateTime.Now;

            if (ModelState.IsValid)
            {
                string fileName = null;
                string thumbName, thumbPath = null;
                string fileSalt = Path.GetRandomFileName().Replace(".", "").ToString();
                string uploadsFolder = Path.Combine(env.WebRootPath, "uploads/images");
                fileName = fileSalt + "_." + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = IOFile.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                siteImage.Image = fileName;
                var image = Image.FromFile(filePath);
                var scaleImage = ImageResize.Scale(image, 480, 480);
                thumbName = "thumb" + "_" + fileName;
                thumbPath = Path.Combine(uploadsFolder, thumbName);
                scaleImage.Save(thumbPath);
                scaleImage.Dispose();
                siteImage.Thumbnail = thumbName;

                try
                {
                    context.SiteImages.Add(siteImage);
                    await context.SaveChangesAsync();
                    return Json(new { Success = true, Message = "File Uploaded" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message });
                }
            }

            return Json(new { Success = false, Message = "Error In Uploading" });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var image = await context.SiteImages.FirstOrDefaultAsync(x => x.Id == id);
            return PartialView(image);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormFile file)
        {
            var fileItem = context.SiteImages.FirstOrDefault(x => x.Id == id);
            string oldFile = fileItem.Image;
            string oldThumb = fileItem.Thumbnail;

            string uploadsFolder = Path.Combine(env.WebRootPath, "uploads/images");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = null;
                    string thumbName, thumbPath = null;
                    string fileSalt = Path.GetRandomFileName().Replace(".", "").ToString();
                    fileName = fileSalt + "_." + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = IOFile.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    fileItem.Image = fileName;
                    var image = Image.FromFile(filePath);
                    var scaleImage = ImageResize.Scale(image, 480, 480);
                    thumbName = "thumb" + "_" + fileName;
                    thumbPath = Path.Combine(uploadsFolder, thumbName);
                    scaleImage.Save(thumbPath);
                    scaleImage.Dispose();
                    fileItem.Thumbnail = thumbName;
                }

                if (await TryUpdateModelAsync(fileItem))
                {
                    fileItem.Modified = DateTime.Now;
                    context.Entry(fileItem).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    
                    if (file != null && file.Length > 0)
                    {
                        if (Utils.FileExists(Path.Combine(uploadsFolder, oldFile)))
                        {
                            Utils.DeleteFile(Path.Combine(uploadsFolder, oldFile));
                        }

                        if (Utils.FileExists(Path.Combine(uploadsFolder, oldThumb)))
                        {
                            Utils.DeleteFile(Path.Combine(uploadsFolder, oldThumb));
                        }
                    }

                    return Json(new { Success = true, Message = "File Updated" });
                }

            }

            return Json(new { Success = false, Message = "Error In Uploading" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var siteimage = await context.SiteImages.FirstOrDefaultAsync(x => x.Id == id);

            string uploadsFolder = Path.Combine(env.WebRootPath, "uploads/images");
            string filePath = Path.Combine(uploadsFolder, siteimage.Image);
            string thumbPath = Path.Combine(uploadsFolder, siteimage.Thumbnail);

            try
            {
                context.SiteImages.Remove(siteimage);
                await context.SaveChangesAsync();

                if (Utils.FileExists(filePath))
                {
                    Utils.DeleteFile(filePath);
                }
                if (Utils.FileExists(thumbPath))
                {
                    Utils.DeleteFile(thumbPath);
                }

                return Json(new { Success = true, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = $"Delete Unsuccessful: {ex.Message}" });
            }
        }

        public async Task<JsonResult> PictureList(int? p)
        {
            var pictures = context.SiteImages;
            var jsonPictures = await PaginatedList<SiteImage>.CreateAsync(pictures, p ?? 1, 16);
            return Json(new { 
                data = jsonPictures,
                hasNextPage = jsonPictures.HasNextPage,
                hasPreviousPage = jsonPictures.HasPreviousPage,
                currentPage = jsonPictures.PageIndex,
                totalPages = jsonPictures.TotalPages
            });
            //return Json(jsonPictures);
        }

        public IActionResult ImageGetter()
        {
            return PartialView();
        }
    }
}