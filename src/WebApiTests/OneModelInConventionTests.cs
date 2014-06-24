using System.Linq;
using System.Web.Http;
using FluentAssertions;
using WebApiPowerTools.Routing;
using WebApiPowerTools.Routing.Conventions;
using Xunit;

namespace WebApiTests
{

    public class OneModelInConventionTests
    {
        [Fact]
        public void route_is_generated_from_input_model()
        {
            var sut = new RoutingConventions();
            sut.Add((IBuildRoutes) new OneModelInHandlerConvention());
            sut.RegisterController<ModelFixtureController>();

            var routes = sut.BuildRoutes();
            routes.Count().Should().Be(1);
            var def = routes.Last();
            def.RouteTemplate.Should().Be("ModelFixture/{page}/{text}/{optional}");
            def.Defaults["controller"].Should().Be("ModelFixture");
            def.Defaults["action"].Should().Be("Get");
            def.Defaults.ContainsKey("page").Should().BeFalse();
            def.Defaults["text"].Should().Be(RouteParameter.Optional);
            def.Defaults["optional"].Should().Be(RouteParameter.Optional);
        }

    }
}