using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;

namespace BlogApp.DataLayer.Concrete.EfCore
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private BlogDbContext _context;
        public EfCategoryRepository(BlogDbContext context)
        {
            _context = context;
        }
        public IQueryable<Category> Categories => _context.Categories;

        public void CreateTag(Category Category)
        {
            _context.Categories.Add(Category);
            _context.SaveChanges();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
