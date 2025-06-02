using BlogApp.DataLayer.Abstract;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlogApp.DataLayer.Concrete.EfCore;
using BlogApp.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public BlogController(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int? categoryId = null)
        {
            int pageSize = 3;
            ViewBag.Categories = await _categoryRepository.Categories.OrderBy(c => c.CategoryName).ToListAsync();

            if (categoryId.HasValue)
            {
                ViewBag.SelectedCategoryId = categoryId.Value;
                ViewBag.SelectedCategory = await _categoryRepository.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId.Value);
            }

            var blogsQuery = _blogRepository.Blogs.Include(b => b.Category).Include(b => b.User).Where(b => b.IsActive);

            if (categoryId.HasValue)
            {
                blogsQuery = blogsQuery.Where(b => b.CategoryId == categoryId.Value);
            }

            blogsQuery = blogsQuery.OrderByDescending(b => b.CreatedDate);
            int totalItems = await blogsQuery.CountAsync();
            int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);
            pageNumber = Math.Clamp(pageNumber, 1, totalPage);

            var blogs = await blogsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.RecentBlogs = await _blogRepository.Blogs.Where(b => b.IsActive).OrderByDescending(b => b.CreatedDate).Take(5).ToListAsync();
            var categoryBlogCounts = await _blogRepository.Blogs.Where(b => b.IsActive)
                .GroupBy(b => b.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

            ViewBag.CategoryBlogCounts = categoryBlogCounts;

            var viewModel = new BlogViewModel
            {
                Blogs = blogs,
                CurrentPage = pageNumber,
                TotalPage = totalPage,
                CategoryId = categoryId
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Category(string url, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(url)) return NotFound();

            var category = await _categoryRepository.Categories.FirstOrDefaultAsync(c => c.Url == url);
            if (category == null) return NotFound();

            int pageSize = 3;
            ViewBag.Categories = await _categoryRepository.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.SelectedCategoryId = category.CategoryId;
            ViewBag.SelectedCategory = category;

            var blogsQuery = _blogRepository.Blogs.Include(b => b.Category).Include(b => b.User)
                .Where(b => b.IsActive && b.CategoryId == category.CategoryId)
                .OrderByDescending(b => b.CreatedDate);

            int totalItems = await blogsQuery.CountAsync();
            int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);
            pageNumber = Math.Clamp(pageNumber, 1, totalPage);

            var blogs = await blogsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.RecentBlogs = await _blogRepository.Blogs.Where(b => b.IsActive).OrderByDescending(b => b.CreatedDate).Take(5).ToListAsync();
            var categoryBlogCounts = await _blogRepository.Blogs.Where(b => b.IsActive)
                .GroupBy(b => b.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

            ViewBag.CategoryBlogCounts = categoryBlogCounts;

            var viewModel = new BlogViewModel
            {
                Blogs = blogs,
                CurrentPage = pageNumber,
                TotalPage = totalPage,
                CategoryId = category.CategoryId
            };

            return View("Index", viewModel);
        }

        public async Task<IActionResult> Details(string url)
        {
            if (string.IsNullOrEmpty(url)) return NotFound();

            var blog = await _blogRepository.Blogs
                .Include(b => b.Category)
                .Include(b => b.User)
                .Include(b => b.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.Url == url && b.IsActive);

            if (blog == null) return NotFound();

            ViewBag.SimilarBlogs = await _blogRepository.Blogs
                .Where(b => b.CategoryId == blog.CategoryId && b.BlogId != blog.BlogId && b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .Take(3)
                .ToListAsync();

            return View(blog);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        [Route("Blog/AddComment")]
        public JsonResult AddComment(int BlogId, string Text)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var username = "DemoUser";
            var avatar = "default.jpg";

            var entity = new Comment
            {
                Text = Text,
                CreatedDate = DateTime.Now,
                BlogId = BlogId,
                UserId = userId
            };

            _commentRepository.CreateComment(entity);
            return Json(new
            {
                username,
                text = Text,
                createdDate = entity.CreatedDate,
                avatar,
            });
        }

        [HttpGet("CreateBlog")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateBlog(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new BlogCreateModel
            {
                Categories = await _categoryRepository.Categories.OrderBy(c => c.CategoryName).ToListAsync()
            };
            return View(model);
        }

        [HttpPost("CreateBlog")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateBlog(BlogCreateModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryRepository.Categories.OrderBy(c => c.CategoryName).ToListAsync();
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!string.IsNullOrEmpty(model.NewCategoryName))
            {
                var existingCategory = await _categoryRepository.Categories
                    .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == model.NewCategoryName.ToLower());

                if (existingCategory != null)
                    model.CategoryId = existingCategory.CategoryId;
                else
                {
                    var newCategory = new Category
                    {
                        CategoryName = model.NewCategoryName,
                        Url = model.NewCategoryName.ToLower().Replace(" ", "-"),
                        Color = CategoryColors.warning
                    };
                    await _categoryRepository.CreateCategoryAsync(newCategory);
                    model.CategoryId = newCategory.CategoryId;
                }
            }

            var blog = new Blog
            {
                Title = model.Title,
                Content = model.Content,
                Description = model.Description,
                Url = model.Url,
                UserId = userId,
                CreatedDate = DateTime.Now,
                Image = "default.jpg",
                IsActive = true,
                CategoryId = model.CategoryId
            };

            _blogRepository.CreateBlog(blog);
            TempData["BlogCreateSucces"] = "Blog yayınlama işleminiz başarılı!";
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost("Blog/DeleteBlog")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogRepository.Blogs.FirstOrDefaultAsync(b => b.BlogId == id);
            if (blog == null) return NotFound();

            blog.IsActive = false;
            _blogRepository.UpdateBlog(blog);
            TempData["BlogDeleteSuccess"] = "Blog başarıyla silindi.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateBlog(int id)
        {
            var blog = await _blogRepository.Blogs.Include(b => b.Category).FirstOrDefaultAsync(b => b.BlogId == id);
            if (blog == null) return NotFound();

            ViewBag.Categories = new SelectList(_categoryRepository.Categories, "CategoryId", "CategoryName");
            var model = new BlogUpdateViewModel
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Content = blog.Content,
                Url = blog.Url,
                CategoryId = blog.CategoryId,
                ImageName = blog.Image,
                IsActive = blog.IsActive
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateBlog(BlogUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "CategoryId", "CategoryName");
                return View(model);
            }

            var blog = await _blogRepository.Blogs.FirstOrDefaultAsync(b => b.BlogId == model.BlogId);
            if (blog == null) return NotFound();

            blog.Title = model.Title;
            blog.Content = model.Content;
            blog.Url = model.Url ?? model.Title.ToLower().Replace(" ", "-");
            blog.CategoryId = model.CategoryId;
            blog.IsActive = model.IsActive;
            blog.UpdatedDate = DateTime.Now;

            _blogRepository.UpdateBlog(blog);
            TempData["BlogUpdateSuccess"] = $"'{blog.Title}' başlıklı blog yazınız başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}
