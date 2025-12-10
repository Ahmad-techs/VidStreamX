using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VidStreamX.Models;

namespace VidStreamX.Models
{
    public enum VideoVisibility
    {
        Public = 0,
        Unlisted = 1,
        Private = 2
    }

    public class Video : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        // The original filename uploaded by user
        [MaxLength(260)]
        public string FileName { get; set; } = null!;

        // The stored blob name in Azure Blob Storage (e.g. guid.mp4)
        [Required, MaxLength(260)]
        public string BlobName { get; set; } = null!;

        // Optional stored base URL (no SAS). For secure delivery use SAS endpoints.
        [MaxLength(1000)]
        public string? BlobUrl { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // View / engagement counters
        public long Views { get; set; } = 0;
        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;

        // Owner
        public Guid OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User? Owner { get; set; }

        // Visibility & status
        public VideoVisibility Visibility { get; set; } = VideoVisibility.Public;

        // Tags many-to-many
        public ICollection<VideoTag>? VideoTags { get; set; }

        // Comments and likes
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }

        // Concurrency token
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
