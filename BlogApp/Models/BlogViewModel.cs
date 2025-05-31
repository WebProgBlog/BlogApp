using BlogApp.Entities;

namespace BlogApp.Models
{
    public class BlogViewModel
    {
        public List<Blog> Blogs { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int? CategoryId { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPage;

    }
}
