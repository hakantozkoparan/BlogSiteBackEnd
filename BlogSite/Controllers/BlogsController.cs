using BlogSite.Context;
using BlogSite.Models;
using BlogSite.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly NewsContext _context;

        public BlogsController(NewsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            var news = _context.Blogs.Select(x => new { x.Id, x.Title, x.Description, x.CategoryId, x.UserId }).ToList();
            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsModel model)
        {
            var news = new Blog
            {
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                UserId = model.UserId
            };
            _context.Blogs.Add(news);
            _context.SaveChanges();
            return Ok("Yazı oluşturuldu.");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] UpdateNewsModel model)
        {
            var news = await _context.Blogs.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            if (model.Title != null)
            {
                news.Title = model.Title;
            }

            if (model.Description != null)
            {
                news.Description = model.Description;
            }

            if (model.isPublish != null)
            {
                news.isPublish = model.isPublish;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var deleted = await _context.Blogs.FindAsync(id);
            if (deleted == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(deleted);
            _context.SaveChanges();
            return Ok("Kayıt silindi.");
        }

        
    }
}
