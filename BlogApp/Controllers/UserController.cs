using BlogApp.DataLayer.Abstract;
using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UserController(IUserRepository userRepository, IBlogRepository blogRepository, ICategoryRepository categoryRepository)
        {
            _userRepository = userRepository;
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyBlogs(int pageNumber = 1)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int pageSize = 4;

            var blogsQuery = _blogRepository.Blogs
                .Include(b => b.Category)
                .Where(b => b.UserId == userId && b.IsActive)
                .OrderByDescending(b => b.CreatedDate);

            int totalItems = await blogsQuery.CountAsync();
            int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);
            pageNumber = Math.Clamp(pageNumber, 1, totalPage);

            var blogs = await blogsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new BlogViewModel
            {
                Blogs = blogs,
                CurrentPage = pageNumber,
                TotalPage = totalPage
            };

            ViewBag.Categories = await _categoryRepository.Categories.ToListAsync();
            return View(viewModel);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound();

            var blogCount = await _blogRepository.Blogs
                .Where(b => b.UserId == userId && b.IsActive)
                .CountAsync();

            var model = new UserProfileViewModel
            {
                Id = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RegisterDate = user.RegisterDate,
                ProfileImage = string.IsNullOrEmpty(user.Image) ? "default-profile.jpg" : user.Image,
                BlogCount = blogCount
            };

            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid) return View("Profile", model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            try
            {
                _userRepository.UpdateUser(user);
                TempData["ProfileUpdateSuccess"] = "Profil bilgileriniz başarıyla güncellendi.";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Profil güncellenirken bir hata oluştu: " + ex.Message);
                return View("Profile", model);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfileImage(IFormFile profileImage)
        {
            if (profileImage == null || profileImage.Length == 0)
            {
                TempData["ProfileImageError"] = "Lütfen bir görsel seçin.";
                return RedirectToAction(nameof(Profile));
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string fileExtension = Path.GetExtension(profileImage.FileName);
            string imageName = $"profile_{Guid.NewGuid()}{fileExtension}";

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "profiles");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var filePath = Path.Combine(uploadsFolder, imageName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                var result = await _userRepository.UpdateUserImage(userId, imageName);

                TempData[result ? "ProfileUpdateSuccess" : "ProfileImageError"] =
                    result ? "Profil resminiz başarıyla güncellendi." : "Profil resmi güncellenirken bir hata oluştu.";
            }
            catch (Exception ex)
            {
                TempData["ProfileImageError"] = "Profil resmi yüklenirken bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}
