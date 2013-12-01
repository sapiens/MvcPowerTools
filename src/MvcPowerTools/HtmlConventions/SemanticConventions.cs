using HtmlTags.Extended.Attributes;

namespace MvcPowerTools.HtmlConventions
{
    public class SemanticConventions : HtmlConventionModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            var editors = conventions.Editors;

            editors.If(d => d.Name.ToLower().Contains("email"))
                .Modify((tag, info) =>
                {
                    tag.FirstInputTag().EmailMode();
                    return tag;
                });

            editors.If(d =>
            {
                var name=d.Name.ToLower();
                return name.Contains("password") || name.Contains("pwd");
            })
                .Modify((tag, info) =>
                {
                    tag.FirstInputTag().PasswordMode();
                    return tag;
                });
        }
    }
}