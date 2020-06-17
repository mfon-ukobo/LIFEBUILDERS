using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LB.Controllers
{
    public class DonateController : Controller
    {
        private readonly Context context;

        public DonateController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}