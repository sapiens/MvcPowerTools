using System;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public static class AngularJsSupport
    {
        /// <summary>
        /// Set and AngularJs directive
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="directive">Name without the "ng-" prefix</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HtmlTag Angular(this HtmlTag tag, string directive, string value=null)
        {
            directive.MustNotBeEmpty();
            return tag.UnencodedAttr("ng-" + directive, value ?? "");
        }

        public static HtmlTag NgModel(this HtmlTag tag, string value)
        {
            return tag.Angular("model", value);
        }

        public static HtmlTag NgController(this HtmlTag tag, string value)
        {
            return tag.Angular("controller", value);
        }

        public static HtmlTag NgInit(this HtmlTag tag, string value)
        {
            return tag.Angular("init", value);
        }

        public static HtmlTag NgClick(this HtmlTag tag, string value)
        {
            return tag.Angular("click", value);           
        }

    }
}