using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAllCategories();
        void CreateCategory(Category category);
        Category GetCategoryById(int id);
        Task CreateCategoryAsync(Category category);
    }
}
