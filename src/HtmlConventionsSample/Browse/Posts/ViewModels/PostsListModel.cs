using HtmlConventionsSample._Auxilia;
using HtmlConventionsSample._Auxilia.Data;
using MvcPowerTools;

namespace HtmlConventionsSample.Browse.Posts.ViewModels
{
    public class PostsListModel:PagedModel<Post>
    {
        public PostsListModel(IPagedInput input=null):base(input)
        {
            
        }
    }
}