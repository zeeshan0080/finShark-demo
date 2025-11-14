
namespace finShark_demo.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public Guid GId { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}