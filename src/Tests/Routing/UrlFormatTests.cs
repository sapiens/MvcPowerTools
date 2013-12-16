using System.Web.Routing;
using MvcPowerTools.Routing;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace XTests.Mvc.Routing
{

   
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
            var pl = new RoutingConventions();
            pl.AddAction(new ActionCall(GetType().GetMethod("test"),GetType(),pl.Settings));
            pl.RegisterHandlerConvention();
            pl.Always().Modify((rt, a) =>
            {
                rt.Url = "heh";
            });
            pl.Apply(r);
            r[0].Cast<Route>().Url.Should().Be("heh");
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}