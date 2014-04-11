using System.Web.Mvc;
using HtmlConventionsSample._Auxilia.Data;
using HtmlConventionsSample._Auxilia.Persistence;
using Ploeh.AutoFixture;

namespace HtmlConventionsSample
{
    public class ConfigTask_AddRandomData
    {
        public static void Run()
        {
            //var db=StaticConfig.Container
            var db = DependencyResolver.Current.GetService<InMemoryDb>();
            var f = new Fixture();
            foreach (var p in f.Build<Post>().Without(d=>d.CreatedOn).CreateMany(5))
            {
                db.Save(p);
            }
        }
    }
}