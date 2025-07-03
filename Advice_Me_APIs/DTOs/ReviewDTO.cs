namespace Advice_Me_APIs.DTOs
{
    public class ReviewDTO
    {
        public int ProductID { get; set; }
        public int RatingStars { get; set; }
        public decimal PurchasePrice { get; set; }
        public string? TextReview { get; set; }
        public string? ProductImage { get; set; }
    }
}
