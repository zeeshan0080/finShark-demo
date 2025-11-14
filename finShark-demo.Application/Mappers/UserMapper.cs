using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs;
using finShark_demo.Application.DTOs.User;
using finShark_demo.Core.Entities;

namespace finShark_demo.Application.Mappers
{
    public class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                GId = user.GId,
                Name = user.Name,
                Email = user.Email,
                IsVerified = user.IsVerified,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public static User ToEntity(RegisterUserDto dto, byte[] passwordHash)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email.ToLower(),
                PasswordHash = passwordHash,
                IsVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateEntity(User user, UpdateUserDto dto)
        {
            user.Name = dto.Name;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
        }
    }
}