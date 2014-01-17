using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CavemanTools;

namespace MvcPowerTools.Extensions
{
	public static class Common
	{

	    public static Pagination ToPagination(this IPagedInput input,int pageSize=15)
	    {
	        return new Pagination(input.Page,pageSize);
	    }

        
        /// <summary>
	    /// Render error view 
	    /// </summary>
	    /// <param name="ctx">HttpContext</param>
	    /// <param name="view">View name</param>
	    /// <param name="viewData">For ViewData</param>
	    public static void HandleError(this HttpContext ctx, string view, object viewData)
	    {
	        if (ctx == null) throw new ArgumentNullException("ctx");
	        if (view == null) throw new ArgumentNullException("view");
	        ctx.Response.Clear();

	        RequestContext rc = ((MvcHandler)ctx.CurrentHandler).RequestContext;
	        string controllerName = rc.RouteData.GetRequiredString("controller");
	        IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
	        IController controller = factory.CreateController(rc, controllerName);
	        ControllerContext cc = new ControllerContext(rc, (ControllerBase)controller);
	        ViewResult viewResult = new ViewResult { ViewName = view };
	        if (viewData != null)
	        {
	            foreach (var kv in viewData.ToDictionary())
	            {
	                viewResult.ViewData.Add(kv.Key, kv.Value);
	            }
	        }
	        viewResult.ExecuteResult(cc);
	        ctx.Server.ClearError();
	    }

	    /// <summary>
		/// Gets the IP of the user  detects proxy
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string RealIp(this HttpRequestBase request)
		{
			if (request == null) throw new ArgumentNullException("request");
			var fip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if ( fip!= null)
			{
				var ip = fip.Split(',').LastOrDefault();
				if (!String.IsNullOrEmpty(ip)) return ip.Trim();				
			}
			return request.UserHostAddress;
		}

        /// <summary>
        /// Returns true if is a POST request
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsPost(this HttpRequestBase req)
        {
            if (req == null) throw new ArgumentNullException("req");
            return req.HttpMethod == "POST";
        }       

	}
	
}