using System.Collections.Generic;

namespace InambeBlog.Models.Post.ViewModels
{
    public class IndexPostVM
    {
        public IEnumerable<PostModel> Posts { get; set; }
        public int PostCount { get; set; }
        public string Query { get; set; }
    }
}
