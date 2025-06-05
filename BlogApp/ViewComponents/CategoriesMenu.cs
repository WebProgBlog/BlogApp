using BlogApp.DataLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class CategoriesMenu : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlogRepository _blogRepository;

        public CategoriesMenu(ICategoryRepository categoryRepository, IBlogRepository blogRepository)
        {
            _categoryRepository = categoryRepository;
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryRepository.Categories
        .OrderBy(c => c.CategoryName)
        .ToListAsync();


            var categoryBlogCounts = await _blogRepository.Blogs
                .Where(b => b.IsActive)
                .GroupBy(b => b.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CategoryId, x => x.Count);


            ViewBag.CategoryBlogCounts = categoryBlogCounts;

            return View(categories);
        }
    }
}
