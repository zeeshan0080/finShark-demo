using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs.Auth;

namespace finShark_demo.Application.Services.Interfaces.Token
{
    public interface ITokenService
    {
        TokenDto GenerateToken(int userId, string email, string name);
        ClaimsPrincipal? ValidateToken(string token);
    }
}