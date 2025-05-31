using BlogApp.DataLayer.Concrete.EfCore;
using BlogApp.Entities;

namespace BlogApp.DataLayer.Abstract
{
 
   
        public interface IBlogRepository
        {
            IQueryable<Blog> Blogs { get; }
            BlogDbContext Context { get; }

            void CreateBlog(Blog blog);
            void UpdateBlog(Blog blog);
            void DeleteBlog(Blog blog);

            Task<bool> UpdateBlogImage(int userId, string imageName);
            Task<bool> SaveImageAsync(IFormFile imageFile, string fileName);



        }
    
}
