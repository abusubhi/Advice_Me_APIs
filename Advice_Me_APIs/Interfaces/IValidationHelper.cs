namespace Advice_Me_APIs.Interfaces
{
    public interface IValidationHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
