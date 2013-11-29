using HtmlTags;

namespace MvcPowerTools.HtmlConventions.Internals
{
    internal class NullBuilder : IBuildElement
    {
        public static IBuildElement Instance = new NullBuilder();

        public bool AppliesTo(ModelInfo info)
        {
            return true;
        }

        public HtmlTag Build(ModelInfo info)
        {
            return HtmlTag.Empty();
        }
    }
}