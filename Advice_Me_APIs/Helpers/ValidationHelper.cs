using Advice_Me_APIs.Interfaces;

namespace Advice_Me_APIs.Helpers
{
    public class ValidationHelper : IValidationHelper
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
