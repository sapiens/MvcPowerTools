using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc.Routing.Constraints;
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
        private Route _route;

        public HandlerRouteConventionTests()
        {
            _sut = new HandlerRouteConvention();
            _ac = new RouteBuilderInfo(new ActionCall(typeof(HomeController).GetMethod("Get")),new RoutingConventions());
            _route = _sut.Build(_ac).First();
            
        }

        [Fact]
        public void builds_proper_get_route()
        {
           _route.Url.Should().Be("home/{id}/{bla}/{title}");
           _route.Constraints["httpMethod"].Cast<HttpMethodConstraint>().AllowedMethods.Contains("GET").Should().BeTrue();
        }

        [Fact]
        public void constraints_are_applied()
        {
            _route.Constraints["id"].Should().BeOfType<IntRouteConstraint>();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}