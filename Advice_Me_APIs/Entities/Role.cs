namespace Advice_Me_APIs.Entities
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; } 

        public ICollection<User> Users { get; set; }
    }
}
