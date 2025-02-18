using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace thaicodelab_api.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User?> AuthenticateUser(string email, string password)
        {
            var user = await _context.tb_users
                .FirstOrDefaultAsync(u => u.email == email && !u.is_deleted);

            if (user == null || !VerifyPassword(password, user.user_password))
            {
                return null;
            }

            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),
                new Claim("user_id", user.user_id.ToString()),
                new Claim("firstname", user.firstname),
                new Claim("lastname", user.lastname),
                new Claim("department_id", user.department_id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                // expires: DateTime.UtcNow.AddHours(2),
                // expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // ใช้ BCrypt สำหรับตรวจสอบรหัสผ่าน
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
        }
    }
}
