using BlogApp.BusinessLayer.Abstract;
using BlogApp.Entities;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllUsers().ToList();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            _userService.DeleteUser(user);
            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var user = _userService.ValidateUser(model.UserName, model.Password);
            if (user == null)
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre" });

            var token = JwtTokenHelper.GenerateToken(user.UserName, user.Role);


            return Ok(new { token });
        }



        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            return Ok(new { message = $"Merhaba {User.Identity?.Name}" });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            var existingUser = _userService.GetAllUsers()
                .FirstOrDefault(u => u.Email == model.Email || u.UserName == model.UserName);

            if (existingUser != null)
                return BadRequest(new { message = "Bu e-posta veya kullanıcı adı zaten kullanılıyor." });

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = model.Role ?? "User",
                RegisterDate = DateTime.Now,

            };

            _userService.CreateUser(user);


            var token = JwtTokenHelper.GenerateToken(user.UserName, user.Role);
            return Ok(new { token });
        }

    }
}
