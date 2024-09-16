using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppP518.Apps.AdminApp.Dtos.ProductDto;
using ShopAppP518.Data;
using ShopAppP518.Entities;

namespace ShopAppP518.Apps.AdminApp.Controllers
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
            var existProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existProduct is null) return NotFound();
            ProductReturnDto productReturnDto = new();
            productReturnDto.Name = existProduct.Name;
            productReturnDto.Id = existProduct.Id;
            productReturnDto.SalePrice = existProduct.SalePrice;
            productReturnDto.CostPrice = existProduct.CostPrice;
            productReturnDto.CreatedDate = existProduct.CreatedDate;
            productReturnDto.UpdatedDate = existProduct.UpdatedDate;
            return Ok(productReturnDto);
        }
        [HttpGet]
        public async Task<IActionResult> Get(string search, int page=1)
        {
            var query = _context.Products
                .Where(p => !p.IsDelete);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));

            ProductListDto productListDto = new();
            productListDto.Page = page;
            productListDto.TotalCount = query.Count();
            productListDto.Items = await query
                .Skip((page - 1) * 2)
                .Take(2)
                .Select(p => new ProductListItemDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SalePrice = p.SalePrice,
                    CostPrice = p.CostPrice,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate
                })
                .ToListAsync();
                
            return Ok(productListDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            Product product = new();
            product.Name = productCreateDto.Name;
            product.SalePrice = productCreateDto.SalePrice;
            product.CostPrice = productCreateDto.CostPrice;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var existProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (existProduct is null) return NotFound();
            existProduct.Name = product.Name;
            existProduct.SalePrice = product.SalePrice;
            existProduct.CostPrice = product.CostPrice;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, bool status)
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
