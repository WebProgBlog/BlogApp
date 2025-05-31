using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter arasında olmalıdır.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 karakter arasında olmalıdır.")]

        public string? LastName { get; set; }


        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100, ErrorMessage = "E-posta 100 karakterden uzun olamaz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifre en az 5 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;



        public string? Image { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public List<Blog> Blogs { get; set; } = new List<Blog>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}