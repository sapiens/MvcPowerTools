using System;
using HtmlTags;
using MvcPowerTools.Html;

namespace HtmlConventionsSample.Html.Display
{
    public class Build:HtmlConventionsModule
    {
        public override void Configure(HtmlConventionsManager conventions)
        {
            var d = conventions.Displays;
            d.ForType<DateTime>()
              .Build(m =>
              {
                  var time = m.Value<DateTime>();
                  var tag = new HtmlTag("span")
                      .AddClass("last-update")
                      .Attr("title", time)
                      .Text(string.Format("Posted {0}", DateTime.Now.Subtract(time).ToHuman()));
                  return tag;
              });
        }
    }
}