using BlogApp.BusinessLayer.Abstract;
using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Concrete
{
    public class BlogManager : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogManager(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public IQueryable<Blog> GetAllBlogs()
        {
            return _blogRepository.Blogs;
        }

        public Blog GetBlogById(int id)
        {
            return _blogRepository.Blogs.FirstOrDefault(b => b.BlogId == id);
        }

        public void CreateBlog(Blog blog)
        {
            _blogRepository.CreateBlog(blog);
        }

        public void UpdateBlog(Blog blog)
        {
            _blogRepository.UpdateBlog(blog);
        }

        public void DeleteBlog(Blog blog)
        {
            _blogRepository.DeleteBlog(blog);
        }

        public Task<bool> UpdateBlogImage(int blogId, string imageName)
        {
            return _blogRepository.UpdateBlogImage(blogId, imageName);
        }

        public Task<bool> SaveImageAsync(IFormFile imageFile, string fileName)
        {
            return _blogRepository.SaveImageAsync(imageFile, fileName);
        }
    }
}
