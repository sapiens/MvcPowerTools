using System;
using System.Linq;
using System.Reflection;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public static class HtmlTagsUtils
    {
        public static T GetChild<T>(this HtmlTag tag) where T : HtmlTag
        {
            var tp = typeof (T);
            return (T) tag.Children.FirstOrDefault(t => ReflectionUtils.IsExactlyType<T>(t));
        }

        public static string IdFromName(string name)
        {
            name.MustNotBeEmpty();
            return name.Replace('.', '_');
        }

        public static HtmlTag CreateValidationTag(this HtmlTag tag, ModelInfo info)
        {
            var errMsg = "";
            if (info.ValidationFailed)
            {
                errMsg = info.ModelErrors[0].ErrorMessage;
            }
            return new ValidationMessageTag(info.HtmlId, info.ValidationFailed, errMsg);
        }      

        /// <summary>
        /// Sets the id of the tag using the asp.net mvc default convention
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static HtmlTag IdFromName(this HtmlTag tag)
        {
            return tag.Id(IdFromName(tag.Attr("name")));
        }

        /// <summary>
        /// Sets the input as checked (checkbox, radio buttons)
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static HtmlTag Checked(this HtmlTag tag)
        {
            if (!tag.IsInputElement()) return tag;
            return tag.Attr("checked", "checked");
        }

        /// <summary>
        /// Searches and returns the input tag contained by the element.
        /// Returns null if not found.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static HtmlTag FirstInputTag(this HtmlTag tag,Predicate<HtmlTag> predicate=null)
        {
            if (predicate == null) predicate = t => true;
            if (tag.IsInputElement() && predicate(tag)) return tag;
            
            return tag.GetChild<HtmlTag>(d => d.IsInputElement() && predicate(d));
        }

        public static bool IsHiddenInput(this HtmlTag tag)
        {
            return tag.IsInputElement() && tag.Attr("type") == "hidden";
        }

        /// <summary>
        /// Searches in all element's children for a tag matching predicate.
        /// Returns null if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static T GetChild<T>(this HtmlTag tag, Predicate<HtmlTag> match) where T : HtmlTag
        {
            var result = tag.Children.FirstOrDefault(d => match(d));
            if (result != null)
            {
                return (T) result;
            }

            foreach (var child in tag.Children)
            {
                return child.GetChild<T>(match);
            }
            return null;
        }

        public static HtmlTag EmailMode(this HtmlTag tag)
        {
            return tag.Attr("type", "email");
        }

        public static HtmlTag NumberMode(this HtmlTag tag)
        {
            return tag.Attr("type", "number");
        }

    }
}