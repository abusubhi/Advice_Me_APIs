using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;

namespace Advice_Me_APIs.Interfaces
{
    public interface IReviewService
    {
        Task<Review> AddReviewAsync(ReviewDTO dto, int userId);
        Task<IEnumerable<ReviewResponseDTO>> GetReviewsByProductIdAsync(int productId);
        Task<bool> ApproveReviewAsync(int reviewId);
        Task<bool> RejectReviewAsync(int reviewId);
    }
}
