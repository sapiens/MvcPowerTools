using System;
using System.Diagnostics;
using System.Web.Mvc;
using FluentAssertions;
using MvcPowerTools.Routing;
using Xunit;

namespace Tests.Routing
{
    class HomeController
    {
        public ActionResult Get(Controller model,int id, int bla = 34,string title = null)
        {
            return null;
        }
    }

    //public class ActionCallTests
    //{
    //    private Stopwatch _t = new Stopwatch();
    //    private ActionCall _sut;

    //    public ActionCallTests()
    //    {
    //        _sut = new ActionCall(typeof (HomeController).GetMethod("Get"), new RoutingConventionsSettings());
    //    }

    //    [Fact]
    //    public void create_route_creates_defaults()
    //    {
    //        var route = _sut.CreateRoute();
    //        route.Url.Should().Be(ActionCall.EmptyRouteUrl);
    //        route.Defaults["controller"].Should().Be("Home");
    //        route.Defaults["action"].Should().Be("Get");
    //    }

    //    [Fact]
    //    public void create_route_has_empty_constraints()
    //    {
    //        var route = _sut.CreateRoute();
    //        route.Constraints.Should().BeEmpty();
    //    }

    //    [Fact]
    //    public void create_action_call()
    //    {
    //        _sut.Controller.Name.Should().Be("HomeController");
    //        _sut.Method.Name.Should().Be("Get");
    //        _sut.Arguments.Count.Should().Be(3);
    //        _sut.Arguments["title"].Should().NotBeNull();
    //    }


    //    [Fact]
    //    public void set_correct_param_defaults()
    //    {
    //        var defaults = _sut.CreateDefaults();
    //        _sut.SetParamsDefaults(defaults);
    //        defaults["controller"].Should().Be("Home");
    //        defaults["action"].Should().Be("Get");
    //        defaults.ContainsKey("id").Should().BeFalse();
    //        defaults["title"].Should().Be(UrlParameter.Optional);
    //        defaults["bla"].Should().Be(34);

    //    }

    //    protected void Write(object format, params object[] param)
    //    {
    //        Console.WriteLine(format.ToString(), param);
    //    }
    //}
}