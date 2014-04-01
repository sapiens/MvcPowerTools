using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using MvcPowerTools.Routing;
using Xunit;

namespace Tests.Routing
{
    public class SomeController : Controller
    {
        public ActionResult Index(int? id, string name = "hi")
        {
            return null;
        }
    }

    public class RoutingConventionsTests
    {
        public RoutingConventionsTests()
        {

        }

        [Fact]
        public void default_builder_yields_no_routes()
        {
            var sut = new RoutingConventions();
            sut.RegisterController<SomeController>();
            var routes = sut.BuildRoutes();
            routes.Should().BeEmpty();

        }

    }
}