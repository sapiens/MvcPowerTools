using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Routing;
using FluentAssertions;
using MvcPowerTools.Routing;
using Xunit;

namespace Tests.Routing
{
    public class HandlerRouteConventionTests
    {
        private Stopwatch _t = new Stopwatch();
        private HandlerRouteConvention _sut;
        private ActionCall _ac;

        public HandlerRouteConventionTests()
        {
            _sut = new HandlerRouteConvention();
            _ac = new ActionCall(typeof(HomeController).GetMethod("Get"),typeof (HomeController), new RoutingConventionsSettings());
            
        }

        [Fact]
        public void test()
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