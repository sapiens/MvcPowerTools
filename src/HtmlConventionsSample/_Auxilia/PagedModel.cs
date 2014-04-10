using CavemanTools.Model;
using MvcPowerTools;

namespace HtmlConventionsSample._Auxilia
{
    public class PagedModel<T> : PagedInput
    {
        public PagedResult<T> Data { get; set; }

        public PagedModel(IPagedInput input=null)
        {
            Data = new PagedResult<T>();
            if (input != null)
            {
                Page = input.Page;
            }
        }
    }
}