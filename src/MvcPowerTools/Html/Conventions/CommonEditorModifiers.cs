using HtmlTags;

namespace MvcPowerTools.Html.Conventions
{
    /// <summary>
    /// Only for primitives
    /// Adds validation attributes,
    /// label (LabelTag) before input 
    /// and 
    /// validation message (ValidationMessageTag) after input
    /// </summary>
    public class CommonEditorModifiers : HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            //conventions.Editors
            //   .If(info => info.Type.Is<DateTime>() || info.Type.Is<DateTime?>())
            //   .Modify((tag, info) =>
            //   {
            //       tag.FirstInputTag().Attr("type", "date");
            //       return tag;
            //   });

            conventions.Editors.IfNotCustomType().Modify(AddValidationAttributes);
            conventions.Editors.IfNotCustomType().Modify(AddEditorLabel);
            conventions.Editors.IfNotCustomType().Modify(AddValidationMessage);
            conventions.Editors.Ignore(d => d.HasAttribute<IgnoreAttribute>());
        }

     

        private static HtmlTag AddValidationAttributes(HtmlTag tag, ModelInfo info)
        {
            var input = tag.FirstNonHiddenInput();
            if (input == null) return tag;
            input.AddValidationAttributes(info);
            return tag;
        }


        private static HtmlTag AddEditorLabel(HtmlTag tag, ModelInfo info)
        {
            if (tag.HasChild<LabelTag>()) return tag;
            var input = tag.FirstNonHiddenInput();
            if (input == null) return tag;
            var label = info.ConventionsRegistry().Labels.GenerateTags(info);
            var parent = input.Parent;
            var res = tag;
            if (parent == null)
            {
                parent = HtmlTag.Placeholder();
                parent.Append(tag);
                res = parent;
            }
            var pos = input.PositionAsChild();
            if (pos == 0) pos = 1;
            label.RegisterParent(parent);
            parent.Children.Insert(pos - 1, label);
            return res;
        }

       
        private static HtmlTag AddValidationMessage(HtmlTag tag, ModelInfo info)
        {
            if (tag.HasChild<ValidationMessageTag>()) return tag;
            var input = tag.FirstNonHiddenInput();
            if (input == null) return tag;
            var validator = info.ConventionsRegistry().Validation.GenerateTags(info);
                //input.CreateValidationTag(info);
            var parent = input.Parent;
            var res = tag;
            if (parent == null)
            {
                parent = HtmlTag.Placeholder();
                parent.Append(tag);
                res = parent;
            }
            var pos = input.PositionAsChild();
            validator.RegisterParent(parent);
            parent.Children.Insert(pos + 1, validator);
            return res;
        }
    }
}