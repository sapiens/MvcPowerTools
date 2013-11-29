using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
    public interface IResultForInvalidModel
    {
        object Data { get; set; }
        ActionResult GetResult(Controller controller);
    }

    public interface IResultForInvalidModel<T> : IResultForInvalidModel where T : class, new()
    {
        T Model { get; set; }
        ISetupModel<T> ModelSetup { get; set; }
    }
}