using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs;
using finShark_demo.Application.DTOs.Auth;
using finShark_demo.Application.DTOs.User;

namespace finShark_demo.Application.Services.Interfaces.User
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByGIdAsync(Guid gid);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<bool> VerifyEmailAsync(Guid gid);
    }
}