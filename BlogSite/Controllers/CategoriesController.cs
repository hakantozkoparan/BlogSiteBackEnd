using BlogSite.Context;
using BlogSite.Models;
using BlogSite.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly NewsContext _context;

        public CategoriesController(NewsContext context)
        {
            _context = context;
        }

        [HttpGet("getcategories")]
        [Authorize(Roles = "Manager")]
        public IActionResult Get()
        {
            var categories = _context.Categories;
            return Ok(categories);
        }

        [HttpPost("createcategory")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryModel model)
        {
            var category = new Category
            {
                Name = model.Name
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(category);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryModel model)
        {

            //Category c = _context.Categories.FirstOrDefault(x => x.Id == model.Id);
            //if (c == null)
            //{
            //    return NotFound();
            //}
            //c.Name = model.Name;
            //_context.Categories.Update(c);
            //_context.SaveChanges();
            //return Ok(c);

            //Category c = (from x in _context.Categories
            //              where x.Id == model.Id
            //              select x).FirstOrDefault();
            //c.Name = model.Name;
            //_context.SaveChanges();
            //return Ok(c);

            _context.Categories.Update(new Category { Id = model.Id, Name = model.Name });
            if (_context.SaveChanges() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok("Kayıt silindi.");
        }

    }
}
