using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using MvcPowerTools;
using Xunit;

namespace Tests
{
    [PropertiesOrder("Title","Id","Email")]
    class WithOrder
    {
        public int Id;
        public string Title { get; set; }
        public string Email { get; set; }
    }
    

  

    public class PropertiesOrderTests
    {
        public PropertiesOrderTests()
        {

        }

        [Fact]
        public void usage_without_specifying_members_throws()
        {
            Assert.Throws<ArgumentException>(() => new PropertiesOrderAttribute());
        }

        [Fact]
        public void members_are_sorted_using_specified_order()
        {
            var members =
                typeof (WithOrder).GetMembers(BindingFlags.Instance | BindingFlags.Public).Where(m=>m.MemberType==MemberTypes.Field || m.MemberType==MemberTypes.Property);
            var sorter = typeof (WithOrder).GetCustomAttribute<PropertiesOrderAttribute>();
            var sorted = sorter.Sort(members);
            sorted.Select(m => m.Name).Should().ContainInOrder(sorter.Properties);
        }
    }
}