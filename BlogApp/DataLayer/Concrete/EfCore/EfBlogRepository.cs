using BlogApp.Entities;
using BlogApp.DataLayer.Abstract;

namespace BlogApp.DataLayer.Concrete.EfCore
{
    public class EfBlogRepository:IBlogRepository
    {
        private BlogDbContext _context;
        public EfBlogRepository(BlogDbContext context)
        {
            _context = context;
        }
        public IQueryable<Blog> Blogs => _context.Blogs;
        public BlogDbContext Context => _context;

        public void CreateBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public void UpdateBlog(Blog blog)
        {
            _context.Blogs.Update(blog);
            _context.SaveChanges();
        }

        public void DeleteBlog(Blog blog)
        {
            blog.IsActive = false;
            _context.Blogs.Update(blog);
            _context.SaveChanges();
        }
        public async Task<bool> UpdateBlogImage(int blogId, string imageName)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null)
                return false;

            blog.Image = imageName;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveImageAsync(IFormFile imageFile, string fileName)
        {
            try
            {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "blogs");


                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    
}
}
