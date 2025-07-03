namespace Advice_Me_APIs.Entities
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string? TextReview { get; set; }
        public int RatingStars { get; set; }
        public decimal PurchasePrice { get; set; }
        public string? ProductImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";

        public Product Product { get; set; }
        public User User { get; set; }
    }

}
