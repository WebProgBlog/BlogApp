using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataLayer.Concrete.EfCore
{
    public class EfCommentRepository : ICommentRepository
    {
        private BlogDbContext _context;
        public EfCommentRepository(BlogDbContext context)
        {
            _context = context;
        }
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public Comment GetCommentById(int id)
        {
            return _context.Comments.FirstOrDefault(c => c.CommentId == id);
        }
        

        public List<Comment> GetCommentsByBlogId(int blogId)
        {
            var comments = _context.Comments
               .Include(c => c.User)
               .Where(c => c.BlogId == blogId)
               .OrderByDescending(c => c.CreatedDate)
               .ToList();

            return comments;

        }
    }
}
