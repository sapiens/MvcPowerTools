using System;
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
                .PropertiesExceptOfType<bool>()
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
                    var wrapper = new DivTag();
                    if (tag is MvcCheckboxElement)
                    {
                        wrapper.AddClass("checkbox");
                    }
                    else
                    {
                        wrapper.AddClass("form-group");
                    }
                    if (model.ValidationFailed)
                    {
                        wrapper.AddClass("has-error");
                    }
                    return tag.WrapWith(wrapper);
                });

        }
    }
}