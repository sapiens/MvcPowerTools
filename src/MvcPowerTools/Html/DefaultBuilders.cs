using HtmlTags;
using HtmlTags.Extended.Attributes;

namespace MvcPowerTools.Html
{
    public static class DefaultBuilders
    {
        /// <summary>
        /// Default type is text. It contains a label, input and validation message span
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static HtmlTag FormInputBuilder(ModelInfo info)
        {
            return new TextboxTag(info.HtmlName, info.ValueAsString).Id(info.HtmlId)
                .Attr("placeholder", info.Meta.Watermark);

            //var tag = HtmlTag.Placeholder();
            //var label = info.ConventionsRegistry().Labels.GenerateTags(info);
            //tag.Append(label);
            //tag.Children.Add();
            //var errMsg = "";
            //if (info.ValidationFailed)
            //{
            //    errMsg = info.ModelErrors[0].ErrorMessage;
            //}
            //tag.Children.Add(new ValidationMessageTag(info.HtmlId, info.ValidationFailed, errMsg));
            //return tag;
        }
        
        public static HtmlTag MvcCheckBoxBuilder(ModelInfo info)
        {
            return new MvcCheckboxElement(info.HtmlId, info.HtmlName, info.RawValue == null ? false : info.Value<bool>());
            //var tag = HtmlTag.Placeholder();
            //var label = info.ConventionsRegistry().Labels.GenerateTags(info);
            
            //tag.Children.Add();
            //var errMsg = "";
            //if (info.ValidationFailed)
            //{
            //    errMsg = info.ModelErrors[0].ErrorMessage;
            //}
            //tag.Append(label);
            //tag.Children.Add(new ValidationMessageTag(info.HtmlId, info.ValidationFailed, errMsg));
            //return tag;
        }

        public static HtmlTag FileUploadBuilder(ModelInfo info)
        {
            var tag = DefaultBuilders.FormInputBuilder(info);
            tag.FirstInputTag().FileUploadMode();
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
            return new HtmlTag("span").Text(info.ValueAsString);
        }
    }
}