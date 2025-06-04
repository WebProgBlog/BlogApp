using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogUpdateViewModel
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Blog baþlýðý zorunludur")]
        [StringLength(100, ErrorMessage = "Baþlýk en fazla 100 karakter olabilir")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Blog içeriði zorunludur")]
        public string Content { get; set; }

        public string? Url { get; set; }

        public int CategoryId { get; set; }

        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }

        public bool IsActive { get; set; } = true;
    }
}