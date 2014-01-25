using System.Web.Mvc;
using HtmlTags;
using HtmlTags.Extended.Attributes;

namespace MvcPowerTools.Html.Conventions
{
    /// <summary>
    /// Builds hidden tag for [HiddenInput]
    /// </summary>
    public class DataAnnotationBuilders : HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            conventions.Editors
                .ForModelWithAttribute<HiddenInputAttribute>()
                .Build(info =>
                {
                    return new HiddenTag()
                        .Name(info.HtmlName)
                        .IdFromName()
                        .Value(info.ValueAsString);
                })
                ;

        }
    }
}