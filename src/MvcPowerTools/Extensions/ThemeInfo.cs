using System;
using System.Web;
using System.Web.Mvc;

namespace MvcPowerTools.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ThemeInfo
    {
        /// <summary>
        /// Just the name. Can be empty. Default is Themes
        /// </summary>
        public static string ThemesDirectoryName = "Themes";
        /// <summary>
        /// Just the name. Can be empty. Default is Style
        /// </summary>
        public static string StyleDirectoryName = "Style";
        /// <summary>
        /// Just the name. Can be empty. Default is Scripts
        /// </summary>
        public static string ScriptsDirectoryName = "Scripts";
        /// <summary>
        /// Just the name. If empty then the views are in the theme directory root.
        /// Default is Views
        /// </summary>
        public static string ViewsDirectoryName = "Views";

        internal ThemeInfo(HttpContextBase ctx,string theme)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");
            Name = theme;
            BaseUrl=UrlHelper.GenerateContentUrl("~/"+ThemesDirectoryName+"/"+Name,ctx);

            StyleUrl = ScriptsUrl =BaseUrl;
            if (!StyleDirectoryName.IsNullOrEmpty())
            {
                StyleUrl+= "/"+StyleDirectoryName;
            }

            if (!ScriptsDirectoryName.IsNullOrEmpty())
            {
                ScriptsUrl += "/" + ScriptsDirectoryName;
            }
            
            if (!ViewsDirectoryName.IsNullOrEmpty())
            {
                ViewsPath = string.Format("~/{1}/{0}/{2}", Name,ThemesDirectoryName,ViewsDirectoryName);
            }
            else
            {
                ViewsPath = "~/{0}/{1}".ToFormat(ThemesDirectoryName, Name);
            }
        }
        /// <summary>
        /// Gets current theme name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the client url for the theme style directory
        /// </summary>
        public string StyleUrl { get; private set; }
        /// <summary>
        /// Gets the client url for the theme scripts directory
        /// </summary>
        public string ScriptsUrl { get; private set; }
        /// <summary>
        /// Gets the client url for the theme 
        /// </summary>
        public string BaseUrl { get; private set; }
        /// <summary>
        /// Relative path of the views directory. Ex: ~/themes/default/views
        /// </summary>
        public string ViewsPath { get; private set; }
    }
}