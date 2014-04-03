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
}