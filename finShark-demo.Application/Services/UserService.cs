using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs;
using finShark_demo.Application.DTOs.Auth;
using finShark_demo.Application.DTOs.User;
using finShark_demo.Application.Mappers;
using finShark_demo.Application.Services.Interfaces.Token;
using finShark_demo.Application.Services.Interfaces.User;
using finShark_demo.Core.Interfaces;
using finShark_demo.Utils;

namespace finShark_demo.Application.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto)
        {
            // Check if email already exists
            if (await _repository.EmailExistsAsync(registerDto.Email))
                throw new ArgumentException("Email already exists");

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var passwordHashBytes = System.Text.Encoding.UTF8.GetBytes(passwordHash);

            // Create user
            var user = UserMapper.ToEntity(registerDto, passwordHashBytes);
            var createdUser = await _repository.AddAsync(user);

            // Generate token
            var token = _tokenService.GenerateToken(createdUser.Id, createdUser.Email, createdUser.Name);
            return SuccessResponse(
                new AuthResponseDto
            {
                Token = token,
                User = UserMapper.ToDto(createdUser),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            }
            );
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _repository.GetByEmailAsync(loginDto.Email);
            
            if (user == null)
                throw new Exception("Invalid email or password");

            if (!user.IsActive)
                throw new ArgumentException("Account is deactivated");

            // Verify password
            var storedHash = System.Text.Encoding.UTF8.GetString(user.PasswordHash);
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, storedHash))
                throw new Exception("Invalid email or password");

            // Generate token
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.Name);

            return SuccessResponse(
                new AuthResponseDto
            {
                Token = token,
                User = UserMapper.ToDto(user),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            }
            );
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                throw new Exception($"User with ID {id} not found");

            return SuccessResponse(UserMapper.ToDto(user));
        }

        public async Task<ApiResponse<UserDto>> GetUserByGIdAsync(Guid gid)
        {
            var user = await _repository.GetByGIdAsync(gid);
            if (user == null)
                throw new Exception($"User with GUID {gid} not found");

            return SuccessResponse(UserMapper.ToDto(user));
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            return SuccessResponse(users.Select(UserMapper.ToDto));
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateDto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                throw new Exception($"User with ID {id} not found");

            UserMapper.UpdateEntity(user, updateDto);
            var updatedUser = await _repository.UpdateAsync(user);
            return SuccessResponse(UserMapper.ToDto(updatedUser));
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
                throw new Exception($"User with ID {id} not found");

            var result = await _repository.DeleteAsync(id);
            return SuccessResponse(result);
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            // Verify current password
            var storedHash = System.Text.Encoding.UTF8.GetString(user.PasswordHash);
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, storedHash))
                throw new ArgumentException("Current password is incorrect");

            // Hash new password
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            user.PasswordHash = System.Text.Encoding.UTF8.GetBytes(newPasswordHash);
            
            await _repository.UpdateAsync(user);
            return SuccessResponse(true, message: "Password changed successfully");
        }

        public async Task<ApiResponse<bool>> VerifyEmailAsync(Guid gid)
        {
            var user = await _repository.GetByGIdAsync(gid);
            if (user == null)
                throw new Exception("User not found");

            user.IsVerified = true;
            await _repository.UpdateAsync(user);
            return SuccessResponse(true, message: "Email verified successfully");
        }
    }
}