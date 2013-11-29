using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.Controllers
{
    public class ViewResultForInvalidModel<T>:AbstractResultForInvalidModel<T> where T : class,new()
    {
        public string ViewName { get; set; }
        public string Layout { get; set; }


        public override ActionResult GetResult(Controller controller)
        {
            if (controller.HttpContext.Request.IsAjaxRequest())
            {
                return new JsonResult(){Data=Data};
            }
            SetupModel();
            if (ViewName.IsNullOrEmpty()) ViewName = controller.GetActionName();
            var result = new ViewResult();
            result.TempData = controller.TempData;
            result.ViewData = controller.ViewData;
            result.MasterName = Layout;
            result.ViewName = ViewName;
            result.ViewData.Model = Data;
            return result;
        }
    }


    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnViewIfValidationFailsAttribute : Attribute, IOverrideValidationFailedPolicy
    {

        /// <summary>
        /// Default is the name of the action
        /// </summary>
        public string ViewName { get; set; }
        public string Layout { get; set; }

        public IResultForInvalidModel<T> GetPolicy<T>(T model) where T : class, new()
        {
            return new ViewResultForInvalidModel<T>()
            {
                ViewName = ViewName,
                Layout = Layout,
                Data = model,
                ModelSetup = DependencyResolver.Current.GetService<ISetupModel<T>>()
            };
        }
    }
}