using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InambeBlog.Helpers;
using InambeBlog.Repositories;
using InambeBlog.Models.Post;
using InambeBlog.Models.Post.ViewModels;

namespace InambeBlog.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private IPostRepo _postRepo;
        private IUserRepo _userRepo;

        public PostController(IPostRepo postRepo, IUserRepo userRepo)
        {
            _postRepo = postRepo;
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [Route("Posts")]
        public IActionResult Index(string query = null, int pageIndex = 1)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            var indexPostVM = new IndexPostVM
            {
                PostCount = _postRepo.Count(query),
                Posts = _postRepo.GetPaginated(pageIndex, true, query),
                Query = query
            };
            return View(indexPostVM);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(CreatePostVM createPostModel)
        {
            if(!ModelState.IsValid)
            {
                return View(createPostModel);
            }
            var postModel = new PostModel()
            {
                AuthorUser = _userRepo.GetById(User.Id()),
                Title = createPostModel.Title,
                Body = createPostModel.Body,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _postRepo.Create(postModel);
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Details(int id)
        {
            var postModel = _postRepo.GetById(id);
            if(postModel == null)
            {
                return NotFound();
            }
            return View(postModel);
        }
        
        public IActionResult Manage()
        {
            var userId = User.Id();
            var user = _userRepo.GetById(userId, true);
            return View(user.UserPosts);
        }
        
        public IActionResult Edit(int id)
        {
            var postModel = _postRepo.GetUserPost(id, User.Id());
            var editPostModel = new EditPostVM
            {
                Id = postModel.Id,
                AuthorUserId = postModel.AuthorUser.Id,
                Title = postModel.Title,
                Body = postModel.Body
            };
            return View(editPostModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditPostVM editPostModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editPostModel);
            }

            if (id != editPostModel.Id || editPostModel.AuthorUserId != User.Id())
            {
                return NotFound();
            }

            var postModel = _postRepo.GetById(id);
            postModel.Title = editPostModel.Title;
            postModel.Body = editPostModel.Body;
            postModel.UpdatedAt = DateTime.Now;

            _postRepo.Context().SaveChanges();

            return RedirectToAction(nameof(Manage), new {
                id
            });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _postRepo.Delete(id);
            return RedirectToAction(nameof(Manage));
        }
    }
}
