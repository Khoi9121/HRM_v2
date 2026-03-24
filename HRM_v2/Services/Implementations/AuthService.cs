using HRM_v2.Data;
using HRM_v2.DTOs;
using HRM_v2.Helpers;
using HRM_v2.Models;
using HRM_v2.Services.Interfaces;
namespace HRM_v2.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public AuthResponseDTO Login(LoginDTO dto)
        {
            var user = _context.UserAccounts
                .FirstOrDefault(x => x.Username == dto.Username);

            if (user == null || user.PasswordHash != dto.Password)
                throw new Exception("Sai tài khoản hoặc mật khẩu");

            var token = JwtHelper.GenerateToken(user.Username, user.Role, _config);

            var refreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            _context.SaveChanges();

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
        public AuthResponseDTO Refresh(string refreshToken)
        {
            var user = _context.UserAccounts
                .FirstOrDefault(x => x.RefreshToken == refreshToken);

            if (user == null)
                throw new Exception("Refresh token không hợp lệ");

            if (user.RefreshTokenExpiryTime < DateTime.Now)
                throw new Exception("Refresh token đã hết hạn");

            // 🔥 tạo token mới
            var newToken = JwtHelper.GenerateToken(user.Username, user.Role, _config);

            // 🔥 tạo refresh token mới (rotation)
            var newRefreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            _context.SaveChanges();

            return new AuthResponseDTO
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
