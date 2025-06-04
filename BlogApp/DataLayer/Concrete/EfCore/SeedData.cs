using Microsoft.EntityFrameworkCore;
using BlogApp.Entities;
using BlogApp.DataLayer.Concrete.EfCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class SeedData
    {
        public static void TestData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<BlogDbContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { CategoryName = "Yazılım", Url = "yazılım", Color = CategoryColors.primary },
                        new Category { CategoryName = "Robotik", Url = "robotik", Color = CategoryColors.warning },
                        new Category { CategoryName = "Gömülü Sistemler", Url = "gomulusistemler", Color = CategoryColors.success },
                        new Category { CategoryName = "Otomasyon", Url = "otomasyon", Color = CategoryColors.info },
                        new Category { CategoryName = "Mobil Geliştirme", Url = "mobilgelistirme", Color = CategoryColors.danger },
                        new Category { CategoryName = "Web Geliştirme", Url = "webgelistirme", Color = CategoryColors.secondary }
                    );
                    context.SaveChanges();
                }



                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "ZeynepOzdes", FirstName = "Zeynep", LastName = "Ozdes", Email = "zozdes@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("zo123"), Image = "pp1.jpg", Role = Roles.Admin, RegisterDate = DateTime.Now.AddMonths(-3) },
                        new User { UserName = "IclalMillik", FirstName = "Iclal", LastName = "Millik", Email = "imillik@email.com", Password = BCrypt.Net.BCrypt.HashPassword("im123"), Image = "pp3.jpg", Role = Roles.User, RegisterDate = DateTime.Now.AddDays(-10) },
                        new User { UserName = "SevvalInal", FirstName = "Sevval", LastName = "Inal", Email = "sinal@email.com", Password = BCrypt.Net.BCrypt.HashPassword("si123"), Image = "pp2.jpg", Role = Roles.User, RegisterDate = DateTime.Now }
                    );
                    context.SaveChanges();
                }

                if (!context.Blogs.Any())
                {

                    context.Blogs.AddRange(
                        new Blog
                        {
                            Title = "ASP.NET Core MVC ile Web Uygulaması Geliştirme",
                            Content = "ASP.NET Core MVC, web uygulamaları geliştirmek için kullanılan bir framework'tür. Bu yazıda ASP.NET Core MVC ile web uygulaması geliştirmenin temel adımlarını inceleyeceğiz.",
                            Description = "ASP.NET Core MVC ile web uygulaması geliştirme adımları",
                            CreatedDate = DateTime.Now.AddDays(-5),
                            UpdatedDate = DateTime.Now,
                            Image = "1.jpg",
                            Url = "aspnet-core-mvc-web-uygulama-gelistirme",
                            IsActive = true,
                            UserId = 1,
                            CategoryId = 1,

                        },
                        new Blog
                        {
                            Title = "Robotik ve Otomasyon Sistemleri",
                            Content = "Robotik ve otomasyon sistemleri, endüstriyel süreçlerin otomatikleştirilmesi için kullanılan teknolojilerdir. Bu yazıda robotik ve otomasyon sistemlerinin temel bileşenlerini inceleyeceğiz.",
                            Description = "Robotik ve otomasyon sistemleri hakkında bilgi",
                            CreatedDate = DateTime.Now.AddDays(-3),
                            UpdatedDate = DateTime.Now,
                            Image = "2.jpg",
                            Url = "robotik-otomasyon-sistemleri",
                            IsActive = true,
                            UserId = 2,
                            CategoryId = 2,

                        },
                        new Blog
                        {
                            Title = "Gömülü Sistemler ve Uygulamaları",
                            Content = "Gömülü sistemler, belirli bir işlevi yerine getirmek için tasarlanmış özel bilgisayarlardır. Bu yazıda gömülü sistemlerin temel bileşenlerini ve uygulama alanlarını inceleyeceğiz.",
                            Description = "Gömülü sistemler ve uygulamaları hakkında bilgi",
                            CreatedDate = DateTime.Now.AddDays(-1),
                            UpdatedDate = DateTime.Now,
                            Image = "3.jpg",
                            Url = "gomulu-sistemler-uygulamalari",
                            IsActive = true,
                            UserId = 3,
                            CategoryId = 3,

                        },
                        new Blog
                        {
                            Title = "C# ile Nesne Yönelimli Programlama",
                            Content = "C#, nesne yönelimli programlama dillerinden biridir. Bu yazıda C# ile nesne yönelimli programlamanın temel kavramlarını inceleyeceğiz.",
                            Description = "C# ile nesne yönelimli programlama",
                            CreatedDate = DateTime.Now.AddDays(-2),
                            UpdatedDate = DateTime.Now,
                            Image = "4.jpg",
                            Url = "csharp-nesne-yonelimli-programlama",
                            IsActive = true,
                            UserId = 1,
                            CategoryId = 1,

                        },
                        new Blog
                        {
                            Title = "Mobil Uygulama Geliştirme",
                            Content = "Mobil uygulama geliştirme, mobil cihazlar için yazılım geliştirme sürecidir. Bu yazıda mobil uygulama geliştirmenin temel adımlarını inceleyeceğiz.",
                            Description = "Mobil uygulama geliştirme adımları",
                            CreatedDate = DateTime.Now.AddDays(-4),
                            UpdatedDate = DateTime.Now,
                            Image = "5.jpg",
                            Url = "mobil-uygulama-gelistirme",
                            IsActive = true,
                            UserId = 2,
                            CategoryId = 5,

                        },
                        new Blog
                        {
                            Title = "Web API ve RESTful Servisler",
                            Content = "Web API ve RESTful servisler, modern web uygulamalarının temelini oluşturan önemli kavramlardır. Bu yazıda Web API geliştirmenin temel prensiplerini inceleyeceğiz.",
                            Description = "Web API ve RESTful servisler hakkında detaylı bilgi",
                            CreatedDate = DateTime.Now.AddDays(-6),
                            UpdatedDate = DateTime.Now,
                            Image = "6.jpeg",
                            Url = "web-api-restful-servisler",
                            IsActive = true,
                            UserId = 3,
                            CategoryId = 6,

                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
