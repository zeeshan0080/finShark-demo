using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finShark_demo.Application.DTOs.Auth
{
    public class TokenDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime AccessTokenExpiresAt { get; set; }
        public required DateTime RefreshTokenExpiresAt { get; set; }
    }
}