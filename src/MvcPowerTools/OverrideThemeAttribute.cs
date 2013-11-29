using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class OverrideThemeAttribute:ActionFilterAttribute
    {

        /// <summary>
        /// Force this theme
        /// </summary>
        /// <param name="theme"></param>
        public OverrideThemeAttribute(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme)) throw new InvalidOperationException("Missing the theme name!");
            Theme = theme;
        }
        
        public string Theme { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.UpdateTheme(Theme);
        }
    }
}