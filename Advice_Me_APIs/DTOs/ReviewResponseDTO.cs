namespace Advice_Me_APIs.DTOs
{
    public class ReviewResponseDTO
    {
        public int ReviewID { get; set; }
        public string? TextReview { get; set; }
        public int RatingStars { get; set; }
        public decimal PurchasePrice { get; set; }
        public string? ProductImage { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
    }
}
