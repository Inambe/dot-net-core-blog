using InambeBlog.Data;
using InambeBlog.Models.Post;
using System.Collections.Generic;

namespace InambeBlog.Repositories
{
    public interface IPostRepo
    {
        PostModel Create(PostModel post);
        IEnumerable<PostModel> GetPaginated(int currentPage, bool relatedData, string query = null);
        PostModel GetById(int id, bool relatedData = false);
        PostModel GetUserPost(int postId, int userId);
        void Delete(int postId);
        int Count(string query = null);
        AppDbContext Context();
    }
}
