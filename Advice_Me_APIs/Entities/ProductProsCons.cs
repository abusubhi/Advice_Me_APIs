namespace Advice_Me_APIs.Entities
{
    public class ProductProsCons
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Type { get; set; } = "Pro"; // or "Con"
        public string Text { get; set; } = string.Empty;

        public Product Product { get; set; }
    }

}
