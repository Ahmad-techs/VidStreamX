using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VidStreamX.Models;

namespace VidStreamX.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid VideoId { get; set; }
        [ForeignKey(nameof(VideoId))]
        public Video? Video { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required, MaxLength(2000)]
        public string Text { get; set; } = null!;

        public DateTime CommentedAt { get; set; } = DateTime.UtcNow;

        // Soft delete
        public bool IsDeleted { get; set; } = false;
    }
}
