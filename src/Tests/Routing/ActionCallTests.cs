using System.Web.Mvc;
using MvcPowerTools.Routing;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace XTests.Mvc.Routing
{
    class HomeController
    {
        public ActionResult Get(ActionCallTests model,int id, int bla = 34,string title = null)
        {
            return null;
        }
    }

    public class ActionCallTests
    {
        private Stopwatch _t = new Stopwatch();
        private ActionCall _sut;

        public ActionCallTests()
        {
            _sut = new ActionCall(typeof (HomeController).GetMethod("Get"),typeof (HomeController), new RoutingConventionsSettings());
        }

        [Fact]
        public void create_route_creates_defaults()
        {
            var route = _sut.CreateRoute();
            route.Url.Should().Be(ActionCall.EmptyRouteUrl);
            route.Defaults["controller"].Should().Be("home");
            route.Defaults["action"].Should().Be("get");
        }

        [Fact]
        public void create_route_has_empty_constraints()
        {
            var route = _sut.CreateRoute();
            route.Constraints.Should().BeEmpty();
        }

        [Fact]
        public void create_action_call()
        {
            _sut.Controller.Name.Should().Be("HomeController");
            _sut.Method.Name.Should().Be("Get");
            _sut.Arguments.Count.Should().Be(3);
            _sut.Arguments["title"].Should().NotBeNull();
        }


        [Fact]
        public void set_correct_param_defaults()
        {
            var defaults = _sut.CreateDefaults();
            _sut.SetParamsDefaults(defaults);
            defaults["controller"].Should().Be("home");
            defaults["action"].Should().Be("get");
            defaults.ContainsKey("id").Should().BeFalse();
            defaults["title"].Should().Be(UrlParameter.Optional);
            defaults["bla"].Should().Be(34);

        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}