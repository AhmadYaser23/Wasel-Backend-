using Microsoft.EntityFrameworkCore;
using project.Helper;
using ProjectWasel.Data;
using ProjectWasel.Helper;
using ProjectWasel.Models;
using ProjectWasel.Models.DTOs;
using ProjectWasel.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWasel.Repositories
{
    public class AuthRepository
    {
        private readonly WaselContext _context;
        private readonly PasswordHasher _passwordHasher;

        public AuthRepository(WaselContext context, PasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // تسجيل مستخدم جديد
        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = "Member",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // تسجيل الدخول
        public async Task<User?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return null;

            bool valid = _passwordHasher.Verify(dto.Password, user.PasswordHash);
            return valid ? user : null;
        }

        // حفظ RefreshToken
        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        // الحصول على المستخدم عبر RefreshToken
        public async Task<User?> GetUserByRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token && !r.IsRevoked && r.ExpiresAt > DateTime.UtcNow);

            return refreshToken?.User;
        }

        // إلغاء RefreshToken
        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
            if (refreshToken == null) return false;

            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}