using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppP518.Apps.AdminApp.Dtos.CategoryDto;
using ShopAppP518.Data;
using ShopAppP518.Entities;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ShopAppContext _context;
    private readonly IMapper mapper;
    public CategoryController(ShopAppContext shopAppContext, IMapper mapper, ShopAppContext context)
    {
        this.mapper = mapper;
        _context = context;
    }
    [HttpGet]

    public async Task<IActionResult> Get(string search, int page = 1)
    {
        var query = _context.Categories.Where(p => !p.IsDelete);
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
        }
        CategoryListDto categoryListDto = new()
        {
            Page = page,
            TotalCount = query.Count(),
            categories = await query.Skip((page - 1) * 2).Take(2)
                     .Select(c => new CategoryListItemDto()
                     {
                         Id = c.Id,
                         Name = c.Name,
                         CreatedDate = c.CreatedDate,
                         UpdateDate = c.UpdatedDate,

                     }).ToListAsync()
        };
        return Ok(categoryListDto);

    }
    [HttpGet("getAll")]
    public IActionResult Get()
    {
        return Ok(_context.Categories.ToList());
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(int? Id)
    {
        if (Id == null) return BadRequest();
        var existedCategory = await _context.Categories.Include(s => s.Products).Where(s => !s.IsDelete).FirstOrDefaultAsync(c => c.Id == Id);
        if (existedCategory == null) return NotFound();
        var categoryReturnDto = mapper.Map<CategoryReturnDto>(existedCategory);
        return Ok(categoryReturnDto);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CategoryCreateDto categoryCreateDto)
    {
        var Isexisted = await _context.Categories.AnyAsync(s => !s.IsDelete && s.Name.ToLower() == categoryCreateDto.Name.ToLower());

        if (Isexisted) return StatusCode(StatusCodes.Status409Conflict);
        Category category = new Category();
        if (categoryCreateDto.Photo is null) return BadRequest();
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryCreateDto.Photo.FileName);
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
        using FileStream fileStream = new(path, FileMode.Create);
        await categoryCreateDto.Photo.CopyToAsync(fileStream);
        category.Name = categoryCreateDto.Name.Trim();
        category.ImageUrl = fileName;
        await _context.Categories.AddAsync(category);
        await  _context.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created);
    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(int? Id, CategoryUpdateDto categoryUpdateDto)
    {
        if (Id == null) return BadRequest();
        var existedCategory = await _context.Categories.Where(s => !s.IsDelete).FirstOrDefaultAsync(c => c.Id == Id);
        if (existedCategory == null) return NotFound();
        var Isexisted = await _context.Categories.AnyAsync(s => !s.IsDelete && s.Name.ToLower() == categoryUpdateDto.Name.ToLower() && existedCategory.Name.ToLower() != categoryUpdateDto.Name.ToLower());
        if (Isexisted) return StatusCode(StatusCodes.Status409Conflict);
        var photo = categoryUpdateDto.Photo;
        if (photo is null) return BadRequest();
        if (!photo.ContentType.Contains("image/")) return BadRequest();
        if (photo.Length / 1024 > 1500) return BadRequest();
        if (!string.IsNullOrEmpty(existedCategory.ImageUrl))
        {
            if (System.IO.File.Exists(existedCategory.ImageUrl))
            {
                System.IO.File.Delete(existedCategory.ImageUrl);
            }
        }
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryUpdateDto.Photo.FileName);
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
        using FileStream fileStream = new(path, FileMode.Create);
        await categoryUpdateDto.Photo.CopyToAsync(fileStream);
        existedCategory.Name = categoryUpdateDto.Name.Trim();
        existedCategory.ImageUrl = fileName;
        _context.Categories.Update(existedCategory);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var existedCategory = await _context.Categories
            .Where(s => !s.IsDelete)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existedCategory == null) return NotFound();

        if (!string.IsNullOrEmpty(existedCategory.ImageUrl) && System.IO.File.Exists(existedCategory.ImageUrl))
        {
            System.IO.File.Delete(existedCategory.ImageUrl);
        }

        _context.Categories.Remove(existedCategory);
        await _context.SaveChangesAsync();
        return Ok();
    }


}