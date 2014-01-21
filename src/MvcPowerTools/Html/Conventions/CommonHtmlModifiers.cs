namespace MvcPowerTools.Html.Conventions
{
    public class CommonHtmlModifiers : HtmlConventionsModule
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
            conventions.Editors
                .Always
                .Modify((tag, info) =>
                {
                    var input = tag.FirstInputTag();
                    if (input == null || input.Attr("type") == "hidden") return tag;
                    MvcHelpers.AddValidationAttributes(input, info);
                    return tag;
                });

        }
    }
}