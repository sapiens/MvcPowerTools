using System;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public class ValidationMessageTag : HtmlTag
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputId"></param>
        /// <param name="validationFailed"></param>
        /// <param name="message">Message is required for invalid input</param>
        public ValidationMessageTag(string inputId, bool validationFailed, string message = "") : base("span")
        {
            if (validationFailed && message.IsNullOrEmpty())
                throw new ArgumentException("Message is required for failed validation");
            AddClass(!validationFailed ? "field-validation-valid" : "field-validation-error")
                .Data("valmsg-for", inputId)
                .Data("valmsg-replace", "true")
                .Text(message);
        }
    }
}