using BlogSite.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Manager")]
[Route("api/[controller]")]
[ApiController]
public class CategoriessController : ControllerBase
{
    private readonly NewsContext _context;

    public CategoriessController(NewsContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = _context.Categories;
        return Ok(categories);
    }
}
