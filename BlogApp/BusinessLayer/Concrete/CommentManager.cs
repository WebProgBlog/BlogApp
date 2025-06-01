using BlogApp.BusinessLayer.Abstract;
using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentManager(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public IQueryable<Comment> GetAllComments()
        {
            return _commentRepository.Comments;
        }

        public void CreateComment(Comment comment)
        {
            _commentRepository.CreateComment(comment);
        }

        public void DeleteComment(Comment comment)
        {
            _commentRepository.DeleteComment(comment);
        }

        public List<Comment> GetCommentsByBlogId(int blogId)
        {
            return _commentRepository.GetCommentsByBlogId(blogId);
        }
    }
}
