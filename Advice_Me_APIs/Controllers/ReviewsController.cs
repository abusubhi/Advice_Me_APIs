using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Advice_Me_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route("addNewReview")]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] ReviewDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _service.AddReviewAsync(dto, userId);
            return CreatedAtAction(nameof(GetReviewsForProduct), new { id = dto.ProductID }, result);
        }


        [HttpGet("/api/products/{id}/reviews")]
        public async Task<IActionResult> GetReviewsForProduct(int id)
        {
            var reviews = await _service.GetReviewsByProductIdAsync(id);
            return Ok(reviews);
        }


        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveReview(int id)
        {
            var result = await _service.ApproveReviewAsync(id);
            return result ? Ok("Review approved.") : NotFound();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectReview(int id)
        {
            var result = await _service.RejectReviewAsync(id);
            return result ? Ok("Review rejected.") : NotFound();
        }
    }

}
