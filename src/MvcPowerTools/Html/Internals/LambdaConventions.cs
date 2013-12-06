using System;
using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    internal class LambdaConventions : IBuildElement, IModifyElement
    {
        public static Predicate<ModelInfo> AppliesAlways = m => true;


        public LambdaConventions(Predicate<ModelInfo> selector)
        {
            Selector = selector;
        }

        public Predicate<ModelInfo> Selector { get; set; }

        public Func<ModelInfo, HtmlTag> Builder { get; set; }

        public Func<HtmlTag, ModelInfo, HtmlTag> Modifier { get; set; }

        public bool AppliesTo(ModelInfo info)
        {
            return Selector(info);
        }

        public HtmlTag Modify(HtmlTag tag, ModelInfo info)
        {
            return Modifier(tag, info);
        }

        public HtmlTag Build(ModelInfo info)
        {
            return Builder(info);
        }
    }
}