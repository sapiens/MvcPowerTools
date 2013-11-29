using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using CavemanTools.Web.Localization;

namespace MvcPowerTools
{
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
	public class LocalizationAttribute:ActionFilterAttribute
	{
		/// <summary>
		/// True to set only UICulture.
		/// Default is false
		/// </summary>
		public bool OnlyUI { get; set; }
		/// <summary>
		/// Query parameter name for the locale
		/// default is 'lang'
		/// </summary>
		public string ParamName { get; set; }
		/// <summary>
		/// Default culture
		/// </summary>
		public CultureInfo Default { get; set; }

		public LocalizationAttribute()
		{
			ParamName = "lang";
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext == null) throw new ArgumentNullException("filterContext");
			var ctx = filterContext.HttpContext;
			var locale = new RequestLocale(ctx.Request.Cookies, ctx.Response.Cookies);
			locale.Caching.CookieName = "_" + ParamName;
			if (locale.LoadFromString(ctx.Request.QueryString[ParamName]))
			{
				locale.Cache();
			}
			else
			{
				locale.LoadFromCache();
			}

			var loc = locale.Value ?? (Default ?? null);
			if (loc!=null)
			{				
				Thread.CurrentThread.CurrentUICulture = loc;
				if (!OnlyUI) Thread.CurrentThread.CurrentCulture = loc;
			}
		}

		
	}
}