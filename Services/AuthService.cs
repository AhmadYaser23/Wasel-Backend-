using ProjectWasel21.Models;
using ProjectWasel21.Models.DTOs;
using ProjectWasel21.Helper;
using ProjectWasel21.Repositories;
using System;
using System.Threading.Tasks;

namespace ProjectWasel21.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepo;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AuthRepository authRepo, JwtHelper jwtHelper)
        {
            _authRepo = authRepo;
            _jwtHelper = jwtHelper;
        }

        // تسجيل مستخدم جديد
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = await _authRepo.RegisterAsync(dto);

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var (rawToken, hashedToken, expiresAt) = _jwtHelper.GenerateRefreshToken();

            await _authRepo.SaveRefreshTokenAsync(new RefreshToken
            {
                UserId = user.UserId,
                Token = hashedToken,
                ExpiresAt = expiresAt,
                IsRevoked = false
            });

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawToken
            };
        }

        // تسجيل دخول
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _authRepo.LoginAsync(dto);
            if (user == null) return null;

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var (rawToken, hashedToken, expiresAt) = _jwtHelper.GenerateRefreshToken();

            await _authRepo.SaveRefreshTokenAsync(new RefreshToken
            {
                UserId = user.UserId,
                Token = hashedToken,
                ExpiresAt = expiresAt,
                IsRevoked = false
            });

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawToken
            };
        }

        // تسجيل الخروج
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            return await _authRepo.RevokeRefreshTokenAsync(refreshToken);
        }

        // تحديث التوكن
        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _authRepo.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null) return null;

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var (rawToken, hashedToken, expiresAt) = _jwtHelper.GenerateRefreshToken();

            await _authRepo.SaveRefreshTokenAsync(new RefreshToken
            {
                UserId = user.UserId,
                Token = hashedToken,
                ExpiresAt = expiresAt,
                IsRevoked = false
            });

            await _authRepo.RevokeRefreshTokenAsync(refreshToken);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawToken
            };
        }
    }
}