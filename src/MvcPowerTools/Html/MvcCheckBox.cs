﻿using HtmlTags;
using HtmlTags.Extended.Attributes;

namespace MvcPowerTools.Html
{
    /// <summary>
    /// Creates a checkbox similar to the one generated by asp.net mvc
    /// </summary>
    public class MvcCheckboxElement : HtmlTag
    {
        public MvcCheckboxElement(string id, string name, bool isChecked) : base("")
        {
            NoTag();
            Tag = new CheckboxTag(isChecked);
            Tag.Id(id).Name(name);
            if (isChecked)
            {
                Tag
                    .Attr("checked", "checked")
                    ;
            }
            Tag.Value("true");
            Children.Add(Tag);
            Children.Add(new HiddenTag().Name(name).Value("false"));
        }

        public CheckboxTag Tag { get; private set; }
    }
}