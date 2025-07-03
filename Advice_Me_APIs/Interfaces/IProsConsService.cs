using Advice_Me_APIs.DTOs;

namespace Advice_Me_APIs.Interfaces
{
    public interface IProsConsService
    {
        Task<IEnumerable<ProsConsDTO>> GetByProductIdAsync(int productId);
        Task AddAsync(ProsConsDTO dto);
    }

}
