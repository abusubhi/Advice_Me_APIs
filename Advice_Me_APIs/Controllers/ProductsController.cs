using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Interfaces;
using Advice_Me_APIs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Advice_Me_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int? categoryId, [FromQuery] string? barcode)
        {
            try
            {
                var result = await _service.SearchAsync(new ProductSearchDTO
                {
                    Name = name,
                    CategoryID = categoryId,
                    Barcode = barcode
                });

                return Ok(result);  // Even if empty, will return 200 OK with []
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // GET: api/products/latest
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestProducts([FromQuery] int count )
        {
            var products = await _service.GetLatestProductsAsync(count);
            return Ok(products);
        }

        // GET: api/products/top-rated
        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRatedProducts([FromQuery] int count )
        {
            var products = await _service.GetTopRatedProductsAsync(count);
            return Ok(products);
        }

        // GET: api/products/most-recommended
        [HttpGet("most-recommended")]
        public async Task<IActionResult> GetMostRecommendedProducts([FromQuery] int count )
        {
            var products = await _service.GetMostRecommendedProductsAsync(count);
            return Ok(products);
        }

        [HttpPost]
        [Route("AddProducts")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] ProductDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var product = await _service.AddAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductID }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _service.ApproveAsync(id);
            return success ? Ok("Product approved.") : NotFound();
        }


        // POST /api/uploads/product-image
        //[HttpPost("/api/uploads/product-image")]
        //[Authorize]
        //public async Task<IActionResult> UploadProductImage([FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
        //    if (!Directory.Exists(uploadsFolder))
        //        Directory.CreateDirectory(uploadsFolder);

        //    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //    var filePath = Path.Combine(uploadsFolder, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    var relativePath = $"/images/products/{fileName}";
        //    return Ok(new { path = relativePath });
        //}

    }

}
