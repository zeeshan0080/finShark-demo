using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finShark_demo.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public Guid GId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}