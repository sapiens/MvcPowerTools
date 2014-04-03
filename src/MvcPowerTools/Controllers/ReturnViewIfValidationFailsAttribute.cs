using System;
using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
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