using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly Context context;
        private readonly UserManager<AppUser> userManager;

        public HomeController(Context context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/admin/dashboard")]
        public IActionResult Dashboard()
        {
            return View(nameof(Index));
        }


        /*************************************** NOTIFICATIONS *************************************/
        public async Task<JsonResult> UnreadNotifications()
        {
            var user = await userManager.Users.Include(x => x.AdminUser).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            var notifications = context.Notifications.Where(x => x.DateTime > user.AdminUser.LastLogin).TakeLast(5);
            return Json(notifications);
        }

        [Route("/admin/notifications")]
        public async Task<IActionResult> Notifications(int? page, int? size)
        {
            var notifications = context.Notifications.OrderByDescending(x => x.Id);
            //return View(await notifications.ToListAsync());
            return View(await PaginatedList<Notification>.CreateAsync(notifications, page ?? 1, size ?? 20));
        }
        public async Task<JsonResult> NotificationList()
        {
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await context.Notifications.FindAsync(id);
            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Notifications));
        }




        /*-------------------------------------- DASHBOARD TASKS ---------------------------------------------*/
        public async Task<JsonResult> Tasks()
        {
            var user = await userManager.Users.Include(x => x.AdminUser).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            var tasks = context.AdminTasks.Where(x => x.AdminUserId == user.Id).OrderBy(x => x.Finished).ThenByDescending(x => x.Id);
            return Json(tasks);
        }

        [HttpPost]
        public async Task<JsonResult> CreateTask(string task)
        {
            var user = await userManager.GetUserAsync(User);
            var adminUser = await context.AdminUsers.FirstOrDefaultAsync(x => x.AppUserId == user.Id);
            adminUser.AdminTasks.Add(new AdminTask
            {
                Created = DateTime.Now,
                Description = task,
                Finished = false
            });

            try
            {
                await context.SaveChangesAsync();
                return Json(new { Success = true, Message = "Task Added" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = $"Task not Added: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTask(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var adminUser = await context.AdminUsers.FirstOrDefaultAsync(x => x.AppUserId == user.Id);
            var task = await context.AdminTasks.FirstOrDefaultAsync(x => x.Id == id && x.AdminUserId == user.Id);

            if (task == null)
            {
                return Json(new { Success = false, Message = "Task not found!" });
            }

            try
            {
                context.AdminTasks.Remove(task);
                await context.SaveChangesAsync();
                return Json(new { Success = true, Message = "Task Removed" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = $"Task not Removed: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ToggleTask(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var adminUser = await context.AdminUsers.FirstOrDefaultAsync(x => x.AppUserId == user.Id);
            var task = await context.AdminTasks.FirstOrDefaultAsync(x => x.Id == id && x.AdminUserId == user.Id);

            if (task == null)
            {
                return Json(new { Success = false, Message = "Task not found!" });
            }

            try
            {
                task.Finished = !task.Finished;
                await context.SaveChangesAsync();
                return Json(new { Success = true, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = $"Failure: {ex.Message}" });
            }
        }



        /******************************************** DASHBOARD SINGLE VALUES **********************/
        public async Task<JsonResult> GetSingleValues()
        {
            try
            {
                var result = new
                {
                    Members = await context.Members.CountAsync(),
                    Posts = await context.Posts.CountAsync(),
                    Events = await context.Events.CountAsync()
                };
                return Json(new { Success = true, Data = result });
            }
            catch (Exception)
            {
                return Json(new { Success = false, Data = "Error retrieving data" });
            }
        }





        /***************************************** SESSION AND ALERTS ***********************************/
        public JsonResult GetError()
        {
            var response = HttpContext.Session.GetObject<SiteResponse>("AppError");
            return Json(response);
        }
    }
}