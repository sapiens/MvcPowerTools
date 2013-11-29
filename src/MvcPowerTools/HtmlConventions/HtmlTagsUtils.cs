using System;
using System.Linq;
using System.Reflection;
using HtmlTags;

namespace MvcPowerTools.HtmlConventions
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
        public static HtmlTag FirstInnerInputTag(this HtmlTag tag)
        {
            return tag.GetChild<HtmlTag>(d => d.IsInputElement());
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


        //public static HtmlTag ValidateAsNumber(this HtmlTag tag, string validationMessage)
        //{
        //   return tag.Validate().Data("val-number", validationMessage);
        //}

        //static HtmlTag Validate(this HtmlTag tag)
        //{
        //    return tag.Data("val", "true");
        //}

        //public static HtmlTag ValidateAsRequired(this HtmlTag tag, string validationMessage)
        //{
        //    return tag.Validate().Data("val-required", validationMessage);
        //}

        //public static HtmlTag ValidateAsRange<T>(this HtmlTag tag, T min, T max,string validationMessage)
        //{
        //    return tag.Validate()
        //        .Data("val-range",validationMessage)
        //        .Data("val-range-max",max)
        //        .Data("val-range-min",min);
        //}

        //public static HtmlTag ValidateAsStringLenth(this HtmlTag tag, int min, int max,string validationMessage)
        //{
        //    return tag.Validate()
        //        .Data("val-length",validationMessage)
        //        .Data("val-length-max",max)
        //        .Data("val-length-min",min);
        //}

        //public static HtmlTag ValidateAsRegex(this HtmlTag tag, string patern, string validationMessage)
        //{
        //    return tag.Validate()
        //        .Data("val-regex", validationMessage)
        //        .Data("val-regex-pattern", patern);
        //}
    }
}