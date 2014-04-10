using MvcPowerTools;

namespace HtmlConventionsSample._Auxilia
{
    public class PagedInput:IPagedInput
    {
        public static int DefaultPageSize = 10;
        public PagedInput()
        {
            Page = 1;
        }
        public int Page { get; set; }
    }
}