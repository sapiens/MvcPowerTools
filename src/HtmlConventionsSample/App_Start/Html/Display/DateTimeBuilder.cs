using System;
using HtmlTags;
using MvcPowerTools.Html;

namespace HtmlConventionsSample.Html.Display
{
    public class DateTimeBuilder : DisplayWidgetBuilder<DateTime>
    {
        public override HtmlTag Build(ModelInfo info)
        {
            var time = info.Value<DateTime>();
            var tag = new HtmlTag("span")
                .AddClass("last-update")
                .Attr("title", time)
                .Text(string.Format("Posted {0}", DateTime.Now.Subtract(time).ToHuman()));
            return tag;
        }
    }
}