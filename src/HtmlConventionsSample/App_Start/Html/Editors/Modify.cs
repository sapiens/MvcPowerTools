using HtmlTags;
using MvcPowerTools.Html;

namespace HtmlConventionsSample.Html.Editors
{
    public class Modify:HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            var e = conventions.Editors;
            TwitterBootstrap(e);
        }

        static void TwitterBootstrap(IDefinedConventions editor)
        {
            editor
                .PropertiesOnly()
                .Modify((tag, model) =>
                {
                    tag.FirstInputTag().AddClass("form-control");
                    return tag;
                })
                ;
            editor
                .PropertiesOnly()
                .Modify((tag, model) =>
                {
                    var wrapper = new DivTag().AddClass("form-group");
                    return tag.WrapWith(wrapper);
                });
        }
    }
}