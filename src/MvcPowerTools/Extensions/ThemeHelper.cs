using System;
using System.Web;
using System.Web.Mvc;

namespace MvcPowerTools.Extensions
{
    public static class ThemeExtensions
    {
        public const string ThemeInfoKey = "_theme-info_";
       
        /// <summary>
        /// Returns the info for the current theme
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ThemeInfo Theme(this WebViewPage page)
        {
            return page.ViewContext.HttpContext.GetThemeInfo();
        }

        /// <summary>
        /// Gets current theme info or null if no theme is set
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ThemeInfo GetThemeInfo(this HttpContextBase ctx)
        {
            var t= ctx.Get<ThemeInfo>(ThemeInfoKey);
            if (t == null)
            {
                var theme = ctx.GetCurrentTheme();
                if (!theme.IsNullOrEmpty())
                {
                    t = new ThemeInfo(ctx,theme);
                    ctx.Items[ThemeInfoKey] = t;
                }
            }
            return t;
        }

        /// <summary>
        /// Returns the name of the current theme
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static string GetCurrentTheme(this HttpContextBase ctx)
        {
            return (string)ctx.Items["theme"];
        }

	    /// <summary>
        /// Returns a link element pointing to the css file
        /// </summary>
        /// <param name="html"></param>
        /// <param name="file"></param>
        /// <returns></returns>       
        public static MvcHtmlString ThemeCss(this HtmlHelper html,string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Filename required");
            var t = html.ViewContext.HttpContext.GetThemeInfo();
            return new MvcHtmlString(string.Format(@"<link href=""{0}"" rel=""stylesheet"" type=""text/css""/>",t.StyleUrl+"/"+file));
        }
     
        /// <summary>
        /// Returns a script element pointing to the specified file
        /// </summary>
        /// <param name="html"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static MvcHtmlString ThemeScript(this HtmlHelper html,string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Filename required");
            var t = html.ViewContext.HttpContext.GetThemeInfo();
            return new MvcHtmlString(string.Format(@"<script src=""{0}"" type=""text/javascript""></script>",t.ScriptsUrl+"/"+file));
        }

        /// <summary>
        /// Sets the current theme name
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="theme"></param>
        public static void UpdateTheme(this HttpContextBase ctx,string theme)
        {
            ctx.Items["theme"] = theme;
        }      

	}
};