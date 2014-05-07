using MvcPowerTools.Html;

namespace HtmlConventionsSample.Html.Labels
{
    public class Modify:HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            var l = conventions.Labels;
            l.Always.Modify((tag, model) =>
            {
                return tag.AddClass("control-label");
            });
        }
    }
}