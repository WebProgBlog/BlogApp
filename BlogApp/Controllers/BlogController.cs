using BlogApp.DataLayer.Abstract;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        

        public BlogController(IBlogRepository blogRepository, ICategoryRepository categoryRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
         
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int? categoryId = null)
        {
            int pageSize = 3;


            ViewBag.Categories = await _categoryRepository.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();


            if (categoryId.HasValue)
            {
                ViewBag.SelectedCategoryId = categoryId.Value;
                ViewBag.SelectedCategory = await _categoryRepository.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryId.Value);
            }


            var blogsQuery = _blogRepository.Blogs
                .Include(b => b.Category)
                .Include(b => b.User)
                .Where(b => b.IsActive);


            if (categoryId.HasValue)
            {
                blogsQuery = blogsQuery.Where(b => b.CategoryId == categoryId.Value);
            }


            blogsQuery = blogsQuery.OrderByDescending(b => b.CreatedDate);


            int totalItems = await blogsQuery.CountAsync();
            int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);


            if (pageNumber < 1)
                pageNumber = 1;
            else if (pageNumber > totalPage && totalPage > 0)
                pageNumber = totalPage;


            var blogs = await blogsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            ViewBag.RecentBlogs = await _blogRepository.Blogs
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .Take(5)
                .ToListAsync();


            var categoryBlogCounts = await _blogRepository.Blogs
                .Where(b => b.IsActive)
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
            if (string.IsNullOrEmpty(url))
            {
                return NotFound();
            }

            var category = await _categoryRepository.Categories
                .FirstOrDefaultAsync(c => c.Url == url);

            if (category == null)
            {
                return NotFound();
            }


            int pageSize = 3;

            ViewBag.Categories = await _categoryRepository.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            ViewBag.SelectedCategoryId = category.CategoryId;
            ViewBag.SelectedCategory = category;

            var blogsQuery = _blogRepository.Blogs
                .Include(b => b.Category)
                .Include(b => b.User)
                .Where(b => b.IsActive && b.CategoryId == category.CategoryId);

            blogsQuery = blogsQuery.OrderByDescending(b => b.CreatedDate);

            int totalItems = await blogsQuery.CountAsync();
            int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageNumber < 1)
                pageNumber = 1;
            else if (pageNumber > totalPage && totalPage > 0)
                pageNumber = totalPage;

            var blogs = await blogsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.RecentBlogs = await _blogRepository.Blogs
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .Take(5)
                .ToListAsync();

            var categoryBlogCounts = await _blogRepository.Blogs
                .Where(b => b.IsActive)
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
            if (string.IsNullOrEmpty(url))
            {
                return NotFound();
            }

            var blog = await _blogRepository.Blogs
                .Include(b => b.Category)
                .Include(b => b.User)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.Url == url && b.IsActive);

            if (blog == null)
            {
                return NotFound();
            }


            ViewBag.SimilarBlogs = await _blogRepository.Blogs
                .Where(b => b.CategoryId == blog.CategoryId && b.BlogId != blog.BlogId && b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .Take(3)
                .ToListAsync();

            return View(blog);
        }
    }
}
