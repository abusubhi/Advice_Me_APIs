using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;

namespace Advice_Me_APIs.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> AddAsync(CategoryDTO dto);
        Task<bool> UpdateAsync(int id, CategoryDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
