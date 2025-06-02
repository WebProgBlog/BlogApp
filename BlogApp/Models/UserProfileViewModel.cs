using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Blog Sayısı")]
        public int BlogCount { get; set; }

        [Display(Name = "Profil Resmi")]
        public string ProfileImage { get; set; }
    }
}
