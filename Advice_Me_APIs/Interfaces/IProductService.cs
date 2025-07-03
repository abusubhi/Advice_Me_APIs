using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;

namespace Advice_Me_APIs.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        public  Task<IEnumerable<ProductDTO>> SearchAsync(ProductSearchDTO search);
        Task<Product> AddAsync(ProductDTO dto, int creatorUserId);
        Task<bool> UpdateAsync(int id, ProductDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveAsync(int id);
        Task<IEnumerable<HomeProductsRecommended>> GetLatestProductsAsync(int count);
        Task<IEnumerable<HomeProductsRecommended>> GetTopRatedProductsAsync(int count);
        Task<IEnumerable<HomeProductsRecommended>> GetMostRecommendedProductsAsync(int count);
    }
}
