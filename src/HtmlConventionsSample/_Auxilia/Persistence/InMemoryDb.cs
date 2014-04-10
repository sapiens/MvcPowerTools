using System;
using System.Collections.Generic;
using HtmlConventionsSample._Auxilia.Data;

namespace HtmlConventionsSample._Auxilia.Persistence
{
    public class InMemoryDb
    {
        private Dictionary<Guid,Post>  _posts=new Dictionary<Guid, Post>();


        public IEnumerable<Post> Posts
        {
            get { return _posts.Values; }
        }

        public void Save(Post post)
        {
            _posts[post.Id] = post;
        }
    }
}