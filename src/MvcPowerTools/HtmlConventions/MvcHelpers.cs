using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HtmlTags;

namespace MvcPowerTools.HtmlConventions
{
    public static class MvcHelpers
    {
        /// <summary>
        /// Populates the input tag with unobtrusive validation attributes
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static HtmlTag AddValidationAttributes(HtmlTag tag, ModelInfo info)
        {
            if (!tag.IsInputElement()) return tag;
            var all = ModelValidatorProviders.Providers.GetValidators(info.Meta, info.ViewContext)
                .SelectMany(d => d.GetClientValidationRules());

            var attr = new Dictionary<string, object>();
            UnobtrusiveValidationAttributesGenerator.GetValidationAttributes(all, attr);
            foreach (var kv in attr)
            {
                tag.Attr(kv.Key, kv.Value);
            }
            return tag;
        }
    }


    

}