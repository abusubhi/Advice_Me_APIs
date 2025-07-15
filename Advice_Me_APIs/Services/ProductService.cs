using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;
using Advice_Me_APIs.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Advice_Me_APIs.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            var productDtoList = products.Select(item => new ProductDTO
            {
                Id = item.ProductID,
                Name = item.Name,
                CategoryID = item.CategoryID,
                ModelNumber = item.ModelNumber,
                Barcode = item.Barcode,
                ImagePath = item.ImagePath,
                Description = item.Description,
                AverageRating = item.AverageRating,
                AveragePrice = item.AveragePrice,
            }).ToList();

            return productDtoList;
        }


        // 1. Latest Products
        public async Task<IEnumerable<HomeProductsRecommended>> GetLatestProductsAsync(int count)
        {
            var products = await _context.Products
                .Where(p => p.Status == "Approved")
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();

            var productDtoList = products.Select(item => new HomeProductsRecommended
            {
                Name = item.Name,
                ModelNumber = item.ModelNumber,
                ImagePath = item.ImagePath,
                AveragePrice = item.AveragePrice,
                AverageRate = item.AverageRating
            });

            return productDtoList;
        }


        // 2. Top Rated Products
        public async Task<IEnumerable<HomeProductsRecommended>> GetTopRatedProductsAsync(int count)
        {
            var products = await _context.Products
                .Where(p => p.Status == "Approved" && p.AverageRating > 3)
                .OrderByDescending(p => p.AverageRating)
                .ThenByDescending(p => p.Reviews.Count)
                .Take(count)
                .ToListAsync();

            var productDtoList = products.Select(item => new HomeProductsRecommended
            {
                Name = item.Name,
                ModelNumber = item.ModelNumber,
                ImagePath = item.ImagePath,
                AveragePrice = item.AveragePrice,
                AverageRate = item.AverageRating
            });

            return productDtoList;
        }


        // 3. Most Recommended Products
        public async Task<IEnumerable<HomeProductsRecommended>> GetMostRecommendedProductsAsync(int count)
        {
            var products = await _context.Products
                .Where(p => p.Status == "Approved")
                .OrderByDescending(p => p.Reviews.Count(r => r.Status == "Approved" && r.RatingStars >= 4))
                .Take(count)
                .ToListAsync();

            var productDtoList = products.Select(item => new HomeProductsRecommended
            {
                Name = item.Name,
                ModelNumber = item.ModelNumber,
                ImagePath = item.ImagePath,
                AveragePrice = item.AveragePrice,
                AverageRate = item.AverageRating
            });

            return productDtoList;
        }


        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return null;

            var productDto = new ProductDTO
            {
                Name = product.Name,
                CategoryID = product.CategoryID,
                ModelNumber = product.ModelNumber,
                Barcode = product.Barcode,
                ImagePath = product.ImagePath,
                Description = product.Description,
                AverageRating = product.AverageRating,
                AveragePrice = product.AveragePrice,
            };

            return productDto;
        }


        public async Task<IEnumerable<ProductDTO>> SearchAsync(ProductSearchDTO search)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            int filterCount = 0;
            if (!string.IsNullOrWhiteSpace(search.Name)) filterCount++;
            if (search.CategoryID.HasValue) filterCount++;
            if (!string.IsNullOrWhiteSpace(search.Barcode)) filterCount++;

            if (filterCount == 0)
                throw new ArgumentException("Please provide one search criteria (Name, CategoryID, or Barcode).");

            if (filterCount > 1)
                throw new ArgumentException("Please search by only one field at a time.");

            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(p => p.Name.Contains(search.Name));

            else if (search.CategoryID.HasValue)
                query = query.Where(p => p.CategoryID == search.CategoryID);

            else if (!string.IsNullOrWhiteSpace(search.Barcode))
                query = query.Where(p => p.Barcode == search.Barcode);

            var result = await query.Select(p => new ProductDTO
            {
                //ProductID = p.ProductID,
                Name = p.Name,
                CategoryID = p.CategoryID,
                //CategoryName = p.Category.Name,
                Barcode = p.Barcode,
                ImagePath = p.ImagePath,
                Description = p.Description,
                AverageRating = p.AverageRating,
                AveragePrice = p.AveragePrice,
                //CreatedAt = p.CreatedAt
            }).ToListAsync();

            return result;
        }




        public async Task<Product> AddAsync(ProductDTO dto, int creatorUserId)
        {
            var exists = await _context.Products.AnyAsync(p => p.Barcode == dto.Barcode);
            if (exists)
                throw new Exception("Product with the same barcode already exists.");

            var product = new Product
            {
                Name = dto.Name,
                CategoryID = dto.CategoryID,
                ModelNumber = dto.ModelNumber,
                Barcode = dto.Barcode,
                ImagePath = dto.ImagePath,
                Description = dto.Description,
                AverageRating = dto.AverageRating,
                AveragePrice = dto.AveragePrice,
                CreatedAt = DateTime.UtcNow,
                //CreatorUserID = creatorUserId,
                Status = "Approved"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(int id, ProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.CategoryID = dto.CategoryID;
            product.ModelNumber = dto.ModelNumber;
            product.Barcode = dto.Barcode;
            product.ImagePath = dto.ImagePath;
            product.Description = dto.Description;
            product.AverageRating = dto.AverageRating;
            product.AveragePrice = dto.AveragePrice;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Status = "Approved";
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
