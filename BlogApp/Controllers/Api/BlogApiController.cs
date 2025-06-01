using Microsoft.AspNetCore.Mvc;
using BlogApp.BusinessLayer.Abstract;
using BlogApp.Entities;

namespace BlogApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogApiController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogApiController(IBlogService blogService)
        {
            _blogService = blogService;
        }

      
        [HttpGet]
        public IActionResult GetAllBlogs()
        {
            var blogs = _blogService.GetAllBlogs().ToList();
            return Ok(blogs);
        }

       
        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            var blog = _blogService.GetBlogById(id);
            if (blog == null) return NotFound();
            return Ok(blog);
        }

        
        [HttpPost]
        public IActionResult CreateBlog([FromBody] Blog blog)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _blogService.CreateBlog(blog);
            return CreatedAtAction(nameof(GetBlog), new { id = blog.BlogId }, blog);
        }

       
        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, [FromBody] Blog blog)
        {
            if (id != blog.BlogId) return BadRequest();
            _blogService.UpdateBlog(blog);
            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            var blog = _blogService.GetBlogById(id);
            if (blog == null) return NotFound();
            _blogService.DeleteBlog(blog);
            return NoContent();
        }
    }
}
