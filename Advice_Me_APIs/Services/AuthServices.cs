using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Entities;
using Advice_Me_APIs.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Advice_Me_APIs.Services.AuthServices;

namespace Advice_Me_APIs.Services
{
    public class AuthServices : IAuth
    {
  private readonly AppDbContext _context;
            private readonly IValidationHelper _validationHelper;
            private readonly ITokenGenerator _tokenGenerator;

            public AuthServices(AppDbContext context, IValidationHelper validationHelper, ITokenGenerator tokenGenerator)
            {
                _context = context;
                _validationHelper = validationHelper;
                _tokenGenerator = tokenGenerator;
            }

            public async Task<string> RegisterAsync(RegistrationDTO dto)
            {
                if (_context.Users.Any(u => u.Email == dto.Email))
                    return "Email is already registered.";

                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = _validationHelper.HashPassword(dto.Password),
                    Age = dto.Age,
                    Gender = dto.Gender,
                    RoleID = dto.RoleID,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "Registered successfully.";
            }

            public async Task<string> LoginAsync(LoginDTO dto)
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (user == null || !_validationHelper.VerifyPassword(dto.Password, user.Password))
                    return "Invalid email or password.";

                var token = _tokenGenerator.GenerateToken(user);
                return token;
            }

        public async Task<GetCurrentUser> GetCurrentUserAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
                return null;

            return new GetCurrentUser
            {
                Id = user.UserID,
                Email = user.Email,
                RoleName = user.Role?.RoleName
            };
        }

    }

}

