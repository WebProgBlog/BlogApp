using BlogApp.BusinessLayer.Abstract;
using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IQueryable<Category> GetAllCategories()
        {
            return _categoryRepository.Categories;
        }

        public void CreateCategory(Category category)
        {
            _categoryRepository.CreateTag(category); // Not: isim olarak `CreateTag` yerine `CreateCategory` olması daha anlamlı olur.
        }

        public Task CreateCategoryAsync(Category category)
        {
            return _categoryRepository.CreateCategoryAsync(category);
        }

     

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

    }
}
