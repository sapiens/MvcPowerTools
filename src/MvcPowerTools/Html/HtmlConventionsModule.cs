namespace MvcPowerTools.Html
{
    public abstract class HtmlConventionsModule
    {
        public HtmlConventionsModule()
        {
            Order = int.MaxValue-100;//to give some space
        }
        /// <summary>
        /// Empty means any profile
        /// </summary>
        public string Profile { get; protected set; }
        public int Order { get; set; }
        public abstract void Configure(HtmlConventionsManager conventions);
    }
}