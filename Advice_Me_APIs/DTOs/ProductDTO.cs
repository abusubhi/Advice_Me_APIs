namespace Advice_Me_APIs.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string ModelNumber { get; set; }
        public string Barcode { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public float AverageRating { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
