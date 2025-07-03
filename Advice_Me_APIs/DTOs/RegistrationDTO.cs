namespace Advice_Me_APIs.DTOs
{
    public class RegistrationDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int RoleID { get; set; } = 1; // Default = User
    }
}
