using BlogApp.DataLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlogRepository _blogRepository;

        public AdminController(IUserRepository userRepository, IBlogRepository blogRepository)
        {
            _userRepository = userRepository;
            _blogRepository = blogRepository;
        }

        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult AllUsers()
        {
            var users = _userRepository.Users.ToList();
            return View(users);
        }

        public IActionResult AllBlogs()
        {
            var blogs = _blogRepository.Blogs.ToList();
            return View(blogs);
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                _userRepository.DeleteUser(user);
                return RedirectToAction("AllUsers");
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteBlog(int id)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.BlogId == id);
            if (blog != null)
            {
                _blogRepository.DeleteBlog(blog);
                return RedirectToAction("AllBlogs");
            }
            return NotFound();
        }
    }
}
