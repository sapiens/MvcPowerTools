using System;
using System.Web.Mvc;
using CavemanTools.Web;

namespace MvcPowerTools
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class ThemeAttribute:ActionFilterAttribute
	{
		/// <summary>
		/// Gets or sets the default theme
		/// </summary>
		public string Default { get; set; }

		/// <summary>
		/// Query parameter name for theme
		/// Default is null, which means the theme can't be changed via url query
		/// </summary>
		public string ParamName { get; set; }

      
		public ThemeAttribute()
		{
			Default = "default";			
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext == null) throw new ArgumentNullException("filterContext");
			var ctx = filterContext.HttpContext;
		    var theme = Default;
            if (!string.IsNullOrEmpty(ParamName))
            {
                var th = new RequestPersonalizationParameter<string>(ParamName);

                if (th.LoadFromString(ctx.Request.QueryString[ParamName]))
                {
                    th.Cache();
                }
                else
                {
                    th.LoadFromCache();
                }
                theme = th.Value ?? Default;
            }
		    ctx.UpdateTheme(theme);
            
		}
	}
}