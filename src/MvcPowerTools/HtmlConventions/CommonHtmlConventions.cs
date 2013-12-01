using System;
using System.Web;

namespace MvcPowerTools.HtmlConventions
{
    public abstract class CommonHtmlConventions : HtmlConventionModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            conventions.Editors
                .If(info => info.Type.DerivesFrom<HttpPostedFileBase>())
                .Build(DefaultBuilders.FileUploadBuilder);
            
            conventions.Labels
                .If(info => info.Type.DerivesFrom<HttpPostedFileBase>())
                .Build(DefaultBuilders.LabelBuilder);
            
            
            conventions.Editors
                .If(info => info.Type.Is<DateTime>() || info.Type.Is<DateTime?>())
                .Modify((tag, info) =>
                {
                    tag.FirstInputTag().Attr("type", "date");
                    return tag;
                });
            conventions.Editors
                .Always
                .Modify((tag, info) =>
                {
                    var input = tag.FirstInputTag();
                    if (tag == null || tag.Attr("type") == "hidden") return tag;
                    MvcHelpers.AddValidationAttributes(input, info);
                    return tag;
                });

        }
    }
}