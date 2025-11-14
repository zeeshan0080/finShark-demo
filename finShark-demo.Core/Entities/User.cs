using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finShark_demo.Core.Entities
{
    public class User : BaseEntity
    {
        public required String Name { get; set; }
        public required String Email { get; set; }
        public Boolean IsVerified { get; set; } = false;
        public Boolean IsActive { get; set; } = true;
        public required byte[] PasswordHash { get; set; }
    }
}