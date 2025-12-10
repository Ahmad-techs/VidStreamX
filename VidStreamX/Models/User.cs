using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VidStreamX.Models;

namespace VidStreamX.Models
{
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(256)]
        public string Email { get; set; } = null!;

        // Hashed password (store securely)
        [Required]
        public string PasswordHash { get; set; } = null!;

        // Display name, profile image, etc.
        [MaxLength(200)]
        public string? DisplayName { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        // Roles (simple comma-separated string for coursework; if using Identity, remove)
        [MaxLength(200)]
        public string? Roles { get; set; }

        // Navigation props
        public ICollection<Video>? Videos { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
