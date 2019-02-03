using System.Collections.Generic;
using System.Linq;
using InambeBlog.Data;
using InambeBlog.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace InambeBlog.Repositories
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _context;

        public PostRepo(AppDbContext context)
        {
            _context = context;
        }

        public PostModel Create(PostModel post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return post;
        }

        public PostModel GetById(int id, bool relatedData = false)
        {
            if (relatedData)
            {
                return _context.Posts
                    .Include(p => p.AuthorUser)
                    .FirstOrDefault(p => p.Id == id);
            }
            return _context.Posts
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<PostModel> GetPaginated(int currentPage, bool relatedData, string query = null)
        {
            var pageSize = Constants.PostPageSize;
            var skip = (currentPage - 1) * pageSize;
            var posts = _context.Posts.AsQueryable();

            posts = query == null
                ?posts
                :posts.Where(p => p.Title.Contains(query));

            posts = posts
                .OrderBy(d => d.Id)
                .Skip(skip)
                .Take(pageSize);
            
            return relatedData
                ?posts.Include(p => p.AuthorUser)
                :posts;
        }

        public PostModel GetUserPost(int postId, int userId)
        {
            return _context.Posts
                .Include(p => p.AuthorUser)
                .FirstOrDefault(p => p.Id == postId && p.AuthorUser.Id == userId);
        }

        public void Delete(int postId)
        {
            var postModel = _context.Posts.FirstOrDefault(p => p.Id == postId);
            _context.Posts.Remove(postModel);
            _context.SaveChanges();
        }

        public AppDbContext Context()
        {
            return _context;
        }

        public int Count(string query = null)
        {
            return query == null
                ?_context.Posts.Count()
                :_context.Posts.Where(p => p.Title.Contains(query)).Count();
        }
    }
}
