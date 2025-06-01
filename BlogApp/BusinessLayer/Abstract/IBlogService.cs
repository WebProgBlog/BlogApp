using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Abstract
{
    public interface IBlogService
    {
        IQueryable<Blog> GetAllBlogs();
        Blog GetBlogById(int id);
        void CreateBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(Blog blog);

        Task<bool> UpdateBlogImage(int blogId, string imageName);
        Task<bool> SaveImageAsync(IFormFile imageFile, string fileName);
    }
}
