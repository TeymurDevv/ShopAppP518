using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppP518.Data;
using ShopAppP518.Entities;

namespace ShopAppP518.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopAppContext _context;

        public ProductController(ShopAppContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id is null) return BadRequest();
            var eixstProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (eixstProduct is null) return NotFound();
            return Ok(eixstProduct);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Products.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created,product);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var existProduct =await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if(existProduct is null) return NotFound();
            existProduct.Name = product.Name;
            existProduct.Price = product.Price;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id,bool status)
        {
            var existProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existProduct is null) return NotFound();
            existProduct.IsDelete = status;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existProduct is null) return NotFound();
            _context.Remove(existProduct);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
