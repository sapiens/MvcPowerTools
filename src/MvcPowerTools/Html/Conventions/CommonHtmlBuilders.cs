using System;
using System.Web;

namespace MvcPowerTools.Html.Conventions
{
    public class CommonHtmlBuilders : HtmlConventionsModule
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
                .ForType<bool>()
                .Build(DefaultBuilders.MvcCheckBoxBuilder);
            conventions.Editors
                .ForType<bool?>()
                .Build(DefaultBuilders.MvcCheckBoxBuilder);
            
           
        }

    }
}