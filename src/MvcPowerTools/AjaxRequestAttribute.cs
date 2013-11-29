using System;
using System.Web.Mvc;

namespace MvcPowerTools
{
    /// <summary>
    /// Marks an action or controller as ajax only
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class)]
    public class AjaxRequestAttribute:Attribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}