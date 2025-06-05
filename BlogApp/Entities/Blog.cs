using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entities
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık 5-200 karakter arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İçerik zorunludur.")]
        [MinLength(10, ErrorMessage = "İçerik en az 10 karakter olmalıdır.")]
        public string Content { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Açıklama 500 karakterden uzun olamaz.")]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "URL zorunludur.")]
        [StringLength(200, ErrorMessage = "URL 200 karakterden uzun olamaz.")]
        [RegularExpression(@"^[a-zA-Z0-9-_]+$", ErrorMessage = "URL sadece harf, rakam, tire ve alt çizgi içerebilir.")]
        public string Url { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Kullanıcı seçilmelidir.")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Comment> Comments { get; set; } = new List<Comment>();

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [StringLength(300)]
        public string? SuggestedTags { get; set; }
    }
}