using BlogApp.Entities;

namespace BlogApp.DataLayer.Abstract
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);
        List<Comment> GetCommentsByBlogId(int blogId);
    }
}
