
namespace Advice_Me_APIs.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string Password { get; set; } 
        public int Age { get; set; }
        public string Gender { get; set; }
        public int RoleID { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Role Role { get; set; }   

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Product> CreatedProducts { get; set; }
    }
}
