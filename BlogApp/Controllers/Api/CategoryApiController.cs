// Controllers/Api/CategoryApiController.cs
using BlogApp.BusinessLayer.Abstract;
using BlogApp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryService.GetAllCategories().ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _categoryService.CreateCategory(category);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }

      

   
    }
}
