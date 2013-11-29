using System;
using HtmlTags;

namespace MvcPowerTools.HtmlConventions.Internals
{
    internal class NopBuilder : IBuildElement
    {
        private readonly HtmlTag _tag;

        public NopBuilder(HtmlTag tag)
        {
            _tag = tag;
        }

        public bool AppliesTo(ModelInfo info)
        {
            throw new NotImplementedException();
        }

        public HtmlTag Build(ModelInfo info)
        {
            return _tag;
        }
    }
}