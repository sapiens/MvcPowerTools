using System;
using System.Reflection;
using System.Web.Mvc;
using FluentAssertions;
using MvcPowerTools;
using MvcPowerTools.Controllers;
using MvcPowerTools.Filters;
using Xunit;

namespace Tests.Filters
{
    
    public class TestMvcController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post( )
        {
            return View();
        }

        public ActionResult Get()
        {
            return View();
        }
    }



    public class TestFilter : ActionFilterAttribute
    {
       
    }

    
    public class FilterConventionsTests
    {
        private FiltersConventions _sut;

        public FilterConventionsTests()
        {
            _sut = new FiltersConventions();
            _sut.RegisterController<TestMvcController>();
            
        }
     

        [Fact]
        public void apply_filter_to_controller_and_action()
        {
            _sut.If(d => d.DeclaringType.Name.StartsWith("Test"))
                .Use<TestFilter>();
            _sut.If(d => d.Name.StartsWith("Post"))
                .Use<AuthorizeAttribute>();

            var provider = _sut.BuildProvider() as FiltersWithPoliciesProvider;
            provider.Filters.Count.Should().Be(3);
            //foreach (var af in provider.Filters)
            //{
            //    Console.WriteLine(af.Key);
            //}
            
        }

    }
}