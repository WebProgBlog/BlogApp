using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Abstract
{
    public interface ICommentService
    {
        IQueryable<Comment> GetAllComments();
        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);

        Comment GetCommentById(int id);
        List<Comment> GetCommentsByBlogId(int blogId);
    }
}
