
namespace Advice_Me_APIs.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; } 
        public string ModelNumber { get; set; } 
        public string Barcode { get; set; } 
        public string ImagePath { get; set; } 
        public string Description { get; set; } 
        public float AverageRating { get; set; }
        public decimal AveragePrice { get; set; }
        public string Status { get; set; } = "Pending";
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; }
        public User Creator { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductProsCons> ProsCons { get; set; }
    }

}
