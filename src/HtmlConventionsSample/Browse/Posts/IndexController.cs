using System.Web.Mvc;
using MvcPowerTools.ControllerHandlers;
namespace HtmlConventionsSample.Browse.Posts
{
    public class IndexController:QueryController<NoInput,NoResult>
    {
        public override ActionResult Get(NoInput input)
        {
            return Content("hi");
        }
    }
}