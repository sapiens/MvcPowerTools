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
        public void route_is_generated_from_input_model()
        {
            var sut = new RoutingConventions();
            sut.UseOneModelInHandlerConvention();
            sut.RegisterController<ModelFixtureController>();
            
            var routes = sut.BuildRoutes();
            routes.Count().Should().Be(1);
            var def = routes.Last();
            def.Url.Should().Be("ModelFixture/{page}/{text}/{optional}");
            def.Defaults["controller"].Should().Be("ModelFixture");
            def.Defaults["action"].Should().Be("Get");
            def.Defaults["page"].Should().BeNull();
            def.Defaults["text"].Should().Be(UrlParameter.Optional);
            def.Defaults["optional"].Should().Be(UrlParameter.Optional);
        }
      
    }
}