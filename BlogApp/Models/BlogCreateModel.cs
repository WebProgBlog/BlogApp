using BlogApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogCreateModel
    {
        public int BLogId { get; set; }
        [Required]
        [Display(Name = "Baþlýk")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Açýklama")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Ýçerik")]
        public string? Content { get; set; }
        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Blog Görseli")]
        public IFormFile? ImageFile { get; set; }
        [Required]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        [Display(Name = "Yeni Kategori")]
        public string? NewCategoryName { get; set; }
        public List<Category>? Categories { get; set; }


    }
}
