using Microsoft.AspNetCore.Mvc;
using InambeBlog.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InambeBlog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(
                nameof(PostController.Index),
                nameof(PostController).ControllerName()
            );
        }
    }
}
