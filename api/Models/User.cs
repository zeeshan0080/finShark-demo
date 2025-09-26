
namespace finShark_demo.Models
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