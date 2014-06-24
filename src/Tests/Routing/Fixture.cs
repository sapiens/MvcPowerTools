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
        public ModelHandlerInput()
        {
            Text = "";
        }
        public int Page { get; set; }
        public string Text { get; set; }
        public int? Optional { get; set; }
    }

    public class ModelFixtureController : Controller
    {
        public ActionResult Get(ModelHandlerInput data)
        {
            return Content("");
        }
    }
}