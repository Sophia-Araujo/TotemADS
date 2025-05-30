using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Data;
using TotemPWA.Models;

namespace TotemPWA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null) 
                .Include(c => c.Subcategories)
                    .ThenInclude(sc => sc.Products)
                .Include(c => c.Products)
                .ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();
            return category;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id) return BadRequest();

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
