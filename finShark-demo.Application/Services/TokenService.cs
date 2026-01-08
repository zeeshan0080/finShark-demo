using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs.Auth;
using finShark_demo.Application.Services.Interfaces.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace finShark_demo.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpiry;
        private readonly int _refreshTokenExpiry;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
            _issuer = _configuration["Jwt:Issuer"] ?? "YourProjectAPI";
            _audience = _configuration["Jwt:Audience"] ?? "YourProjectClient";
            if (!int.TryParse(_configuration["Jwt:AccessTokenExpirationMinutes"], out _accessTokenExpiry))
            {
                _accessTokenExpiry = 10;
            }

            if (!int.TryParse(_configuration["Jwt:RefreshTokenExpirationMinutes"], out _refreshTokenExpiry))
            {
                _refreshTokenExpiry = 60;
            }

        }

        public static String GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public TokenDto GenerateToken(int userId, string email, string name)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            DateTime accessTokenExpiryTime = DateTime.UtcNow.AddMinutes(_accessTokenExpiry);
            DateTime refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_refreshTokenExpiry);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = accessTokenExpiryTime,
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpiryTime,
                RefreshTokenExpiresAt = refreshTokenExpiryTime
            };
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = false, ///Set true for validity check
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}