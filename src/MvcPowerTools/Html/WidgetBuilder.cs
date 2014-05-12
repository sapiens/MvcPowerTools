using HtmlTags;

namespace MvcPowerTools.Html
{
    public abstract class WidgetBuilder<T> : IBuildElement
    {
        public bool AppliesTo(ModelInfo info)
        {
            return info.Type == typeof(T);
        }

        public abstract HtmlTag Build(ModelInfo info);

        protected T Model(ModelInfo info)
        {
            return info.Value<T>();
        }
    }
}