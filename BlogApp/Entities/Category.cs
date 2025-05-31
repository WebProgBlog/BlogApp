using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entities
{
    public enum CategoryColors
    {
        primary, danger, warning, success, secondary, info
    }
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Kategori adı 2-100 karakter arasında olmalıdır.")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Açıklama 500 karakterden uzun olamaz.")]
        public string? Description { get; set; }

        public string? CategoryImage { get; set; }

        [Required(ErrorMessage = "URL zorunludur.")]
        [StringLength(200, ErrorMessage = "URL 200 karakterden uzun olamaz.")]
        [RegularExpression(@"^[a-zA-Z0-9-_]+$", ErrorMessage = "URL sadece harf, rakam, tire ve alt çizgi içerebilir.")]
        public string Url { get; set; } = null!;
        public CategoryColors? Color { get; set; }
        public List<Blog> Blogs { get; set; } = new List<Blog>();
    }
}