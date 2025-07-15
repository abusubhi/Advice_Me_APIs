using Advice_Me_APIs.Entities;
using Advice_Me_APIs.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Advice_Me_APIs.Services
{
    public class UsersService: IUsers
    {
        private readonly AppDbContext _context;
        public UsersService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
    }
}
