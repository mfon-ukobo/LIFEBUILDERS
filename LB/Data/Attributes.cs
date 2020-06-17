using LB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB
{
	public class GetErrorsAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			Controller controller = (Controller) context.Controller;
			SiteResponse response = context.HttpContext.Session.GetObject<SiteResponse>("AppError");
			controller.ViewData["AppError"] = response;
			base.OnActionExecuted(context);
		}
	}

	public class PaginationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			Controller controller = (Controller)context.Controller;
			controller.ViewData["search"] = controller.Request.Query["search"].ToString() ?? null;
			base.OnActionExecuted(context);
		}
	}
}
