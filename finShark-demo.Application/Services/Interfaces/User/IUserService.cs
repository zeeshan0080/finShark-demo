using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs;
using finShark_demo.Application.DTOs.Auth;
using finShark_demo.Application.DTOs.User;
using finShark_demo.Utils;

namespace finShark_demo.Application.Services.Interfaces.User
{
    public interface IUserService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
        Task<ApiResponse<UserDto>> GetUserByGIdAsync(Guid gid);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateDto);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
        Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<ApiResponse<bool>> VerifyEmailAsync(Guid gid);
    }
}