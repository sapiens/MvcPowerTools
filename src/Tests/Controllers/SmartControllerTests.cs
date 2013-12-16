using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Web.Mvc;
using Moq;
using MvcPowerTools.Controllers;
using MvcPowerTools.Controllers.Internal;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit.Extensions;

namespace XTests.Mvc.Controllers
{
    public class MyClass
    {
        public int Id { get; set; }
    }
    public class SmartControllerTests
    {
        private Stopwatch _t = new Stopwatch();

      
        [Fact]
        public void only_one_override_policy()
        {
            var action = new Mock<ICustomAttributeProvider>();
            action.Setup(d => d.GetCustomAttributes(true))
                  .Returns(new IOverrideValidationFailedPolicy[] {new ReturnViewIfValidationFailsAttribute(), new TransferIfValidationFailsAttribute()});
            var facade = new SmartContextFacade();
            ValidModelOnlyAttribute.CheckOverridePolicy(action.Object,facade);
            facade.PolicyOverride.Should().BeOfType<ReturnViewIfValidationFailsAttribute>();
        }

        [Fact]
        public void no_overridebles()
        {
             var action = new Mock<ICustomAttributeProvider>();
            action.Setup(d => d.GetCustomAttributes(true))
                  .Returns(new IOverrideValidationFailedPolicy[0]);
            var facade = new SmartContextFacade();
            ValidModelOnlyAttribute.CheckOverridePolicy(action.Object, facade);
            facade.PolicyOverride.Should().BeNull();
        }

        [Fact]
        public void model_is_first_by_default()
        {
            dynamic bag = new ExpandoObject();
            bag.Model = new MyClass(){Id = 3};
            var model = (MyClass)ValidModelOnlyAttribute.EstablishModel(bag);
            model.Should().NotBeNull();
            model.Id.Should().Be(3);
        }


        [Fact]
        public void model_position_is_specified_by_attribute()
        {
            dynamic bag = new ExpandoObject();
            bag.Id = 22;
            bag.Model = new MyClass() { Id = 3 };
            AssertionExtensions.Should(
                (object) ValidModelOnlyAttribute.EstablishModel(bag, new ModelIsArgumentAttribute(1))).BeOfType<MyClass>();
        }
        
        [Fact]
        public void model_name_is_specified_by_attribute()
        {
            dynamic bag = new ExpandoObject();
            bag.Id = 22;
            bag.Model = new MyClass() { Id = 3 };
            AssertionExtensions.Should(
                (object) ValidModelOnlyAttribute.EstablishModel(bag, new ModelIsArgumentAttribute("Model"))).BeOfType<MyClass>();
        }

        [Theory]
        [InlineData(22)]
        [InlineData("bla")]
        [InlineData(JsonRequestBehavior.AllowGet)]
        [InlineData(new byte[0])]
        public void model_should_be_reference_type(dynamic value)
        {
            dynamic bag = new ExpandoObject();
            bag.Id = value;
            Assert.Null(ValidModelOnlyAttribute.EstablishModel(bag));            
        }

        protected void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }



    }

   
}