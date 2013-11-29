using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CavemanTools.Web.Helpers;

namespace MvcPowerTools.Extensions
{
    public static class HtmlHelpers
    {
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> source,Func<T,SelectListItem> lambda)
        {
            if (source==null || !source.Any()) yield break;
            foreach (var x in source) yield return lambda(x);
        }
     

        /// <summary>
        /// Creates page navigation links as an ul with CSS class=pager
        /// </summary>
        /// <param name="html">helper</param>
        /// <param name="page">Current page</param>
        /// <param name="itemsOnPage">Number of items displayed on a page</param>
        /// <param name="totalItems">Total number of items available</param>
        /// <param name="linkHrefFormat">link format for paging navigation </param>
        /// <param name="currentFormat">format for current page navigation.Default it renders
        /// &lt;span class="current"&gt;{0}&lt;span&gt;</param>
        /// <param name="ulClass">additional CSS class for the ul</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper html, int page, int itemsOnPage, int totalItems, Func<int,string> linkHrefFormat,
                                                Func<int,string> currentFormat = null, string ulClass = "")
        {
            var pl = new PaginationLinks();
            pl.Current = page;
            pl.ItemsOnPage = itemsOnPage;
            pl.TotalItems = totalItems;
            pl.LinkUrlFormat = linkHrefFormat;
            pl.CurrentPageFormat = currentFormat ?? pl.CurrentPageFormat;
            var sb = new StringBuilder();
            if (ulClass != "") ulClass = " " + ulClass;
            sb.AppendFormat("<ul class=\"pager{0}\">", ulClass);
            foreach (var link in pl.GetPages())
            {
                sb.AppendFormat("<li>{0}</li>", link);
            }
            sb.Append("</ul>");
            return new MvcHtmlString(sb.ToString());
        }

     
    }
}