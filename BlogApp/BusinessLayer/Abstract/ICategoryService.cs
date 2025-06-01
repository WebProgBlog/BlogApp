using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAllCategories();
        void CreateCategory(Category category);
        Task CreateCategoryAsync(Category category);
    }
}
