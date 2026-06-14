using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finShark_demo.Application.DTOs.User
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}