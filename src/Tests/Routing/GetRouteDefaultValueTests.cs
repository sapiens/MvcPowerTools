using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using MvcPowerTools.Routing;
using Xunit;

namespace Tests.Routing
{
    public class GetRouteDefaultValueTests
    {
        [Fact]
        public void when_type_is_valueType_and_has_default_value_then_theres_no_default()
        {
            var model = new ModelHandlerInput() { Page = 0 };
            var data = new RouteValueDictionary();
            Extensions.SetDefaultValue(typeof(ModelHandlerInput).GetProperty("Page"), model, data);

            data.ContainsKey("page").Should().BeFalse();            
        }
        
        [Fact]
        public void when_type_is_valueType_and_has_non_default_value_then_use_that_value()
        {
            var model = new ModelHandlerInput() { Page = 234 };
            var data = new RouteValueDictionary();
            Extensions.SetDefaultValue(typeof(ModelHandlerInput).GetProperty("Page"), model, data);

            data["page"].Should().Be(234);
        }

        [Fact]
        public void when_nullable_property_and_null_value_then_its_optional()
        {
            var model = new ModelHandlerInput() { Optional = null };
            var data = new RouteValueDictionary();
            Extensions.SetDefaultValue(typeof(ModelHandlerInput).GetProperty("Optional"), model, data);

            data["optional"].Should().Be(UrlParameter.Optional);
        }

        [Fact]
        public void when_nullable_property_with_value_then_use_that_value()
        {
            var model = new ModelHandlerInput() { Optional = 45};
            var data = new RouteValueDictionary();
            Extensions.SetDefaultValue(typeof(ModelHandlerInput).GetProperty("Optional"), model, data);

            data["optional"].Should().Be(45); 
        }

        [Fact]
        public void when_string_property_and_null_then_theres_no_default()
        {
            var model = new ModelHandlerInput() {Text = null };
            var data = new RouteValueDictionary();
            Extensions.SetDefaultValue(typeof(ModelHandlerInput).GetProperty("Text"), model, data);
            data.ContainsKey("text").Should().BeFalse();   
        }

        [Fact]
        public void when_string_property_and_non_empty_then_use_value()
        {
            var model = new ModelHandlerInput() { Text = "test" };
            var data = new RouteValueDictionary();

            Extensions.SetDefaultValue(typeof (ModelHandlerInput).GetProperty("Text"), model, data);
            
            data["text"].Should().Be("test");
        }

        [Fact]
        public void when_string_property_with_empty_value_then_its_optional()
        {
            var model = new ModelHandlerInput() { Text = "" };
            var data = new RouteValueDictionary();

            Extensions.SetDefaultValue(typeof(ModelHandlerInput).GetProperty("Text"), model, data);

            data["text"].Should().Be(UrlParameter.Optional);
        }
    }
}