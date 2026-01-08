using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using finShark_demo.Application.DTOs.User;

namespace finShark_demo.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        // public UserDto User { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}