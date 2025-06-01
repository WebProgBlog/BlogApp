using BlogApp.Entities;

namespace BlogApp.DataLayer.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void CreateTag(Category Category);
        void GetCategoryById(int id);
        Task CreateCategoryAsync(Category category);
    }
}
