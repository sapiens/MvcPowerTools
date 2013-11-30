using HtmlTags;

namespace MvcPowerTools.HtmlConventions
{
    public static class DefaultBuilders
    {
        /// <summary>
        /// Default type is text
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static HtmlTag TextboxBuilder(ModelInfo info)
        {
            var tag = HtmlTag.Placeholder();
            var label = info.ConventionsRegistry().Labels.GenerateTags(info);
            tag.Append(label);
            var value = info.RawValue == null ? "" : info.RawValue.ToString();
            tag.Children.Add(new TextboxTag(info.HtmlName, value).Id(info.HtmlId));
            var errMsg = "";
            if (info.ValidationFailed)
            {
                errMsg = info.ModelErrors[0].ErrorMessage;
            }
            tag.Children.Add(new ValidationMessageTag(info.HtmlId, info.ValidationFailed, errMsg));
            return tag;
        }

        /// <summary>
        /// Creates label tag having for and name set
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static HtmlTag LabelBuilder(ModelInfo info)
        {
            if (info.IsRootModel) return HtmlTag.Empty();
            return new LabelTag(info.HtmlId, info.Name);    
        }

        /// <summary>
        /// Creates a span
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static HtmlTag BasicTagBuilder(ModelInfo info)
        {
            return new HtmlTag("span").Text(info.RawValue==null?"":info.RawValue.ToString());
        }
    }
}