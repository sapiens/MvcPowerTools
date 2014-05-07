using System;
using System.Web.Mvc;
using HtmlTags;

namespace MvcPowerTools.Html.Conventions
{
    /// <summary>
    /// Modifiers to make your form elements look nice. 
    /// You want this added after data annotation modifiers but before other modifiers.
    /// </summary>
    public class TwitterBootstrapFormElements:HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            Editors(conventions.Editors);
            Labels(conventions.Labels);
        }

        void Labels(IDefinedConventions label)
        {
            label.Always.Modify((tag, model) => tag.AddClass("control-label"));
        }

        void Editors(IDefinedConventions editor)
        {
            editor
                .PropertiesExceptOfType<bool>()
                .Modify((tag, model) =>
                {
                    var input = tag.FirstInputTag();
                    if (input == null || input.IsHiddenInput()) return tag;
                    input.AddClass("form-control");
                    return tag;
                })
                ;
            editor.
                If(m =>
                !m.IsRootModel && !m.Type.IsUserDefinedClass() && !m.HasAttribute<HiddenInputAttribute>())
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