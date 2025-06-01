// Controllers/Api/CommentApiController.cs
using BlogApp.BusinessLayer.Abstract;
using BlogApp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentApiController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var comments = _commentService.GetAllComments().ToList();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public IActionResult Create(Comment comment)
        {
            _commentService.CreateComment(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.CommentId }, comment);
        }

       

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null) return NotFound();
            _commentService.DeleteComment(comment);
            return NoContent();
        }
    }
}
