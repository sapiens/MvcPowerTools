using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using MvcPowerTools.Routing;
using Xunit;


namespace Tests.Routing
{
    
    public class OneModelInConventionTests
    {
        [Fact]
        public void get_route_is_generated_from_input_model()
        {
            var sut = new RoutingConventions();
            sut.UseOneModelInHandlerConvention();
            sut.RegisterController<ModelFixtureController>();
            
            var routes = sut.BuildRoutes();
            routes.Count().Should().Be(2);
            var def = routes.First();
            def.Url.Should().Be("ModelFixture/{page}/{text}/{optional}");
            def.Defaults["controller"].Should().Be("ModelFixture");
            def.Defaults["action"].Should().Be("Get");
            def.Defaults.ContainsKey("page").Should().BeFalse();
            def.Defaults["text"].Should().Be(UrlParameter.Optional);
            def.Defaults["optional"].Should().Be(UrlParameter.Optional);
        }
      
        [Fact]
        public void post_route_ignores_input_model()
        {
            var sut = new RoutingConventions();
            sut.UseOneModelInHandlerConvention();
            sut.RegisterController<ModelFixtureController>();
            
            var routes = sut.BuildRoutes();
            routes.Count().Should().Be(2);
            var def = routes.Last();
            def.Url.Should().Be("ModelFixture");
            def.Defaults["controller"].Should().Be("ModelFixture");
            def.Defaults["action"].Should().Be("Post");
            def.Defaults.ContainsKey("page").Should().BeFalse();
            def.Defaults.ContainsKey("text").Should().BeFalse();
            def.Defaults.ContainsKey("optional").Should().BeFalse();
            
        }
      

    }
}