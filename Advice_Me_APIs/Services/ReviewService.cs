using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;
using Advice_Me_APIs.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Advice_Me_APIs.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddReviewAsync(ReviewDTO dto, int userId)
        {
            var review = new Review
            {
                ProductID = dto.ProductID,
                UserID = userId,
                RatingStars = dto.RatingStars,
                PurchasePrice = dto.PurchasePrice,
                TextReview = dto.TextReview,
                ProductImage = dto.ProductImage,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductID == productId && r.Status == "Approved")
                .Select(r => new ReviewResponseDTO
                {
                    ReviewID = r.ReviewID,
                    TextReview = r.TextReview,
                    RatingStars = r.RatingStars,
                    PurchasePrice = r.PurchasePrice,
                    ProductImage = r.ProductImage,
                    CreatedAt = r.CreatedAt,
                    Status = r.Status,
                    UserName = r.User.Name
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return false;

            review.Status = "Approved";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return false;

            review.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
