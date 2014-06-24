using System.Web.Mvc;
using MvcPowerTools.Controllers;
using MvcPowerTools.Filters;

namespace HtmlConventionsSample.Filters
{
    public class Filters:FiltersConventionsModule
    {
        public override void Configure(IConfigureFilters filters)
        {
            filters.If(m => m.Name.StartsWith("Post"))
                .Use<ValidModelOnlyAttribute>();
        }
    }
}