using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Routing;
using FluentAssertions;
using MvcPowerTools.Routing;
using MvcPowerTools.Routing.Conventions;
using Xunit;

namespace Tests.Routing
{
    public class HandlerRouteConventionTests
    {
        private Stopwatch _t = new Stopwatch();
        private HandlerRouteConvention _sut;
        private RouteBuilderInfo _ac;

        public HandlerRouteConventionTests()
        {
            _sut = new HandlerRouteConvention();
            _ac = new RouteBuilderInfo(new ActionCall(typeof(HomeController).GetMethod("Get")),new RoutingConventions());
            
        }

        [Fact]
        public void builds_proper_get_route()
        {
            var route = _sut.Build(_ac).First();
            route.Url.Should().Be("home/{id}/{bla}/{title}");
            route.Constraints["httpMethod"].Cast<HttpMethodConstraint>().AllowedMethods.Contains("GET").Should().BeTrue();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}