using System;

namespace VidStreamX.Models
{
    public abstract class BaseEntity
    {
        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;
    }
}
