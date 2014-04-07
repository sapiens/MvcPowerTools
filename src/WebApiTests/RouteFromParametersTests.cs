using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Routing.Constraints;
using FluentAssertions;
using WebApiPowerTools.Routing;
using WebApiPowerTools.Routing.Conventions;
using Xunit;

namespace WebApiTests
{
    public class MyController : ApiController
    {
        public IEnumerable<string> GetSomething(int id,ApiController user,string data="hi")
        {
            return Enumerable.Empty<string>();
        }
    }

    public class RouteFromParametersTests
    {
        private RouteFromParametersConvention _sut;
        private IHttpRoute _route;

        public RouteFromParametersTests()
        {
            var conventions = new RoutingConventions();
            _sut = new RouteFromParametersConvention();
            var routes = _sut.Build(new RouteBuilderInfo(new ActionCall(typeof(MyController).GetMethod("GetSomething")), conventions));
            _route = routes.First();
        }

        [Fact]
        public void route_template_is_generated_properly()
        {
            _route.RouteTemplate.Should().Be("my/getsomething/{id}/{data}");
        }

        [Fact]
        public void int_constraint_is_applied()
        {
            _route.Constraints["id"].Should().BeOfType<IntRouteConstraint>();
        }

        [Fact]
        public void one_default_has_value()
        {
            _route.Defaults.Count.Should().Be(3);//controller + action = 2
            _route.Defaults["data"].Should().Be("hi");
        }

        [Fact]
        public void user_defined_args_are_ignored()
        {
            _route.Defaults.ContainsKey("user").Should().BeFalse();
        }

    }
}