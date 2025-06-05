
using BlogApp.DataLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class LastPosts : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public LastPosts(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentBlogs = await _blogRepository.Blogs
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .Take(5)
                .ToListAsync();

            return View(recentBlogs);
        }
    }
}