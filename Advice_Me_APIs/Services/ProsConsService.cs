using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;
using Advice_Me_APIs.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Advice_Me_APIs.Services
{
    public class ProsConsService : IProsConsService
    {
        private readonly AppDbContext _context;

        public ProsConsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProsConsDTO>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductProsCons
                .Where(p => p.ProductID == productId)
                .Select(p => new ProsConsDTO
                {
                    ProductID = p.ProductID,
                    Type = p.Type,
                    Text = p.Text
                }).ToListAsync();
        }

        public async Task AddAsync(ProsConsDTO dto)
        {
            var entry = new ProductProsCons
            {
                ProductID = dto.ProductID,
                Type = dto.Type,
                Text = dto.Text
            };
            _context.ProductProsCons.Add(entry);
            await _context.SaveChangesAsync();
        }
    }

}
