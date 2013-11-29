using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines
{
    /// <summary>
    /// Any implementation will be used as a singleton
    /// </summary>
    public interface IFindViewConvention
    {
        bool Match(ControllerContext context, string viewName);
        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        string GetViewPath(ControllerContext controllerContext, string viewName);
        /// <summary>
        /// Gets relative path for master (layout). If master name is empty, it should return empty
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        string GetMasterPath(ControllerContext controllerContext, string masterName);
    }
}