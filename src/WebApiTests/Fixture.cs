using System.Web.Http;

namespace WebApiTests
{

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

    public class ModelFixtureController : ApiController
    {
        public string Get(ModelHandlerInput data)
        {
            return "";
        }
    }
}