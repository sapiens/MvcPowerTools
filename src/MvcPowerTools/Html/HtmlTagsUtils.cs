using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public static class HtmlTagsUtils
    {
        /// <summary>
        /// Creates and returns a select tag (dropdown box) having the enumeration as options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="setOption"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static SelectTag ToSelectTag<T>(this IEnumerable<T> options,Action<SelectTag,T> setOption, object selectedValue=null)
        {
            options.MustNotBeNull();
            setOption.MustNotBeNull();
        
            var tag = new SelectTag();
            foreach (var option in options)
            {
                setOption(tag, option);
            }
            if (selectedValue!=null)
            tag.SelectByValue(selectedValue);
            return tag;
        }

        /// <summary>
        /// Creates and returns a select tag (dropdown box) having the enumeration as options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="textValue">Lambda to select a display value</param>
        /// <param name="optionValue">Lambda to select the option value</param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static SelectTag ToSelectTag<T>(this IEnumerable<T> options, Func<T, string> textValue,
            Func<T, object> optionValue,object selectedValue=null)
        {
            return ToSelectTag(options, (s, d) => s.Option(textValue(d), optionValue(d)),selectedValue);
        }


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

        //public static ValidationMessageTag CreateValidationTag(this HtmlTag tag, ModelInfo info)
        //{
        //    var errMsg = "";
        //    if (info.ValidationFailed)
        //    {
        //        errMsg = info.ModelErrors[0].ErrorMessage;
        //    }
        //    return new ValidationMessageTag(info.HtmlId, info.ValidationFailed, errMsg);
        //}

        public static FormTag POST(this FormTag tag)
        {
            return tag.Method("POST");
        }
        
        public static FormTag GET(this FormTag tag)
        {
            return tag.Method("GET");
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

        public static HtmlTag FirstNonHiddenInput(this HtmlTag tag)
        {
            return tag.FirstInputTag(d => !d.IsHiddenInput());
        }

        public static bool HasChild<T>(this HtmlTag tag, T instance = null) where T:HtmlTag
        {
            if (instance == null) return tag.GetChild<T>() != null;
            return tag.GetChild<T>(t => t == instance) != null;
        }

        /// <summary>
        /// Since HtmlTag doesn't have a Parent setter, we have to use this hack to set a tag's parent
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static HtmlTag RegisterParent(this HtmlTag tag, HtmlTag parent)
        {
            parent.MustNotBeNull();
            parent.Append(tag);
            parent.Children.Remove(tag);
            return tag;
        }


        /// <summary>
        /// Gets the position of tag relative to parent. -1 means it doesn't have a parent
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int PositionAsChild(this HtmlTag tag)
        {
            if (tag.Parent == null) return -1;
            for (var i = 0; i < tag.Parent.Children.Count; i++)
            {
                if (tag.Parent.Children[i] == tag) return i;
            }
            throw new InvalidOperationException("This is a bug or this tag was removed from its parent by another thread");
        }

        public static bool IsHiddenInput(this HtmlTag tag)
        {
            return tag.IsInputElement() && tag.Attr("type") == "hidden";
        }

        /// <summary>
        /// Returns the tag's parent or a placeholder containing the tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static HtmlTag ParentOrPlaceholder(this HtmlTag tag)
        {
            if (tag.Parent == null) return tag.WrapWith(HtmlTag.Placeholder());
            return tag.Parent;
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