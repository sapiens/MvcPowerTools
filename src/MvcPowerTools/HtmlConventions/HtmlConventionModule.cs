namespace MvcPowerTools.HtmlConventions
{
    public abstract class HtmlConventionModule
    {
        public HtmlConventionModule()
        {
            Order = int.MaxValue;
        }
        /// <summary>
        /// Empty means default profile
        /// </summary>
        public string Profile { get; protected set; }
        public int Order { get; set; }
        public abstract void Configure(HtmlConventionsManager conventions);
    }
}