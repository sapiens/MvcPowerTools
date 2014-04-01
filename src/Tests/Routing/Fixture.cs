using System.Web.Mvc;

namespace Tests.Routing
{

   public class HandlerFixtureController:Controller
    {
        public ActionResult Get(int page=3)
        {
            return Content("");
        }
    }

    public class ModelHandlerInput
    {
        public int Page { get; set; }
    }

    public class ModelFixtureController : Controller
    {
        public ActionResult Get(ModelHandlerInput data)
        {
            return Content("");
        }
    }
}