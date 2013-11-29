using System.Web.Routing;
using MvcPowerTools.Routing;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace XTests.Mvc.Routing
{

    public class Prefix : IRouteUrlFormatPolicy
    {
        public bool Match(ActionCall action)
        {
            return true;
        }

        public string Format(string url, ActionCall actionInfo)
        {
            return "heh";
        }
    }
    public class UrlFormatTests
    {
        private Stopwatch _t = new Stopwatch();

        public UrlFormatTests()
        {

        }

        [Fact]
        public void test()
        {
            var r = RouteTable.Routes;
            var pl = new RoutingPolicy();
            pl.AddAction(new ActionCall(GetType().GetMethod("test"),pl.Settings));
            pl.RegisterHandlerConvention();
            pl.UrlFormatPolicies.Add(new Prefix());
            pl.Apply(r);
            r[0].Cast<Route>().Url.Should().Be("heh");
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}