namespace BlogApp.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public int BlogId { get; set; }
        public Blog? Blog { get; set; } = null!;
        public int UserId { get; set; }
        public User? User { get; set; } = null!;
    }
}
