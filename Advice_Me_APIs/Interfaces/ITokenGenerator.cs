using Advice_Me_APIs.Entities;

namespace Advice_Me_APIs.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);

    }
}
