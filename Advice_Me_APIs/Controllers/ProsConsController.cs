using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Advice_Me_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProsConsController : ControllerBase
    {
        private readonly IProsConsService _service;

        public ProsConsController(IProsConsService service)
        {
            _service = service;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var result = await _service.GetByProductIdAsync(productId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] ProsConsDTO dto)
        {
            if (dto.Type != "Pro" && dto.Type != "Con")
                return BadRequest("Type must be 'Pro' or 'Con'");

            await _service.AddAsync(dto);
            return Ok("Added successfully.");
        }
    }

}
