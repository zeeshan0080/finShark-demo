using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finShark_demo.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public Guid GId { get; set; } = Guid.NewGuid();
        public String Name { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public Boolean IsVerified { get; set; } = false;
        public Boolean IsActive { get; set; } = true;
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}