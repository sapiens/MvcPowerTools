using HtmlTags;

namespace MvcPowerTools.Html.Conventions
{
    /// <summary>
    /// Adds a label before display value
    /// </summary>
    public class CommonDisplayModifiers : HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            conventions.Displays.Ignore(d => d.HasAttribute<IgnoreAttribute>());
            conventions.Displays.IfNotCustomType()
                .Modify(AddFieldLabel);

        }

        public static HtmlTag AddFieldLabel(HtmlTag tag, ModelInfo info)
        {
            var label=new SpanTag().Text(info.Name).AddClass("display-label");
            //if (tag.HasChild<LabelTag>()) return tag;
            //var label = info.ConventionsRegistry().Labels.GenerateTags(info);
            tag.WrapWith(HtmlTag.Placeholder()).InsertFirst(label);
            return tag.Parent;
        }
    }
}