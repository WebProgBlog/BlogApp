using BlogApp.BusinessLayer.Abstract;
using BlogApp.Entities;
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

            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
