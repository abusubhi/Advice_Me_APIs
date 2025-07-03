using Advice_Me_APIs.DTOs;

namespace Advice_Me_APIs.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegistrationDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
        Task<GetCurrentUser> GetCurrentUserAsync(int userId);
    }
}
