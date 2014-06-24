
using MvcPowerTools.Html;

namespace HtmlConventionsSample.Html.Validation
{
    public class Modify:HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            conventions.Validation.Always.Modify((tag, model) =>
            {
                return tag.AddClass("bg-danger");
            });
        }
    }
}