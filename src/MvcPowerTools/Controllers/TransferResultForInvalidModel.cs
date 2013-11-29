using System;
using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
    public class TransferResultForInvalidModel<T>:AbstractResultForInvalidModel<T> where T : class, new()
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public override ActionResult GetResult(Controller controller)
        {
            SetupModel();
            return new TransferToActionResult(ActionName,ControllerName,controller.RouteData.Values);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class TransferIfValidationFailsAttribute : Attribute, IOverrideValidationFailedPolicy
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public IResultForInvalidModel<T> GetPolicy<T>(T model) where T : class, new()
        {
            return new TransferResultForInvalidModel<T>()
            {
                ActionName = ActionName,
                ControllerName = ControllerName,
                Model = model,
                ModelSetup = DependencyResolver.Current.GetService<ISetupModel<T>>()
            };
        }
    }
    
}