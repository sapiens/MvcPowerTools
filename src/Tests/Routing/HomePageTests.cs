
using System.Linq;
using FluentAssertions;
using MvcPowerTools.Routing;
using Xunit;

namespace Tests.Routing
{
    public class HomePageTests
    {
       
        public HomePageTests()
        {
           
           
        }


        [Fact]
        public void handles_argument_list_properly()
        {
            var sut = new RoutingConventions();
            sut.UseHandlerConvention();
            sut.RegisterController<HandlerFixtureController>();
            var id = 2;
            sut.HomeIs<HandlerFixtureController>(f => f.Get(id));
            var routes = sut.BuildRoutes();
            routes.Count().Should().Be(2);
            var def = routes.Last();
            def.Url.Should().Be("{*catch}");
            def.Defaults["controller"].Should().Be("HandlerFixture");
            def.Defaults["action"].Should().Be("Get");
            def.Defaults["page"].Should().Be(2);                       
        }
        
        /// <summary>
        /// Test to see if it works with user defined class passed as action arg. Used by <see cref="MvcPowerTools.Routing.Conventions.OneModelInHandlerConvention"/>
        /// </summary>
        [Fact]
        public void handles_one_model_in_argument_properly()
        {
            var sut = new RoutingConventions();
            sut.UseOneModelInHandlerConvention();
            sut.RegisterController<ModelFixtureController>();
            var id = 2;
            sut.HomeIs<ModelFixtureController>(f => f.Get(new ModelHandlerInput(){Page = 67}));
            var routes = sut.BuildRoutes();
            routes.Count().Should().Be(2);
            var def = routes.Last();
            def.Url.Should().Be("{*catch}");
            def.Defaults["controller"].Should().Be("ModelFixture");
            def.Defaults["action"].Should().Be("Get");
            def.Defaults["page"].Should().Be(67);                       
        }
    }
}