namespace Advice_Me_APIs.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; }
    }

}
