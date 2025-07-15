using Advice_Me_APIs.Entities;

namespace Advice_Me_APIs.Interfaces
{
    public interface IUsers
    {
        Task<List<User>> GetAllUsersAsync();

    }
}
