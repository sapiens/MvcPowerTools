using System.Web.Mvc;
using CavemanTools;
using FluentAssertions;
using MvcPowerTools.ViewEngines;
using MvcPowerTools.ViewEngines.Conventions;
using Xunit;

namespace Tests.ViewEngine
{

    [Order(0)]
    public class First : BaseViewConvention
    {
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            throw new System.NotImplementedException();
        }
    }

    [Order(1)]
    public class Second : BaseViewConvention
    {
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Last : BaseViewConvention
    {
        public override string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            throw new System.NotImplementedException();
        }
    }



    public class ExtensionsTests
    {
        private FlexibleViewEngineSettings _sut;

        public ExtensionsTests()
        {
            _sut = new FlexibleViewEngineSettings();
            _sut.Conventions.Clear();
        }

        [Fact]
        public void conventions_are_added_in_the_specified_order()
        {
            _sut.RegisterConventions(typeof (ExtensionsTests).Assembly);
            _sut.Conventions[0].Should().BeOfType<First>();
            _sut.Conventions[1].Should().BeOfType<Second>();
            _sut.Conventions[2].Should().BeOfType<Last>();
        }

    }
}