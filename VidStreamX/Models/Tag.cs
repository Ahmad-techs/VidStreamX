using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VidStreamX.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        // Videos that use this tag (many-to-many)
        public ICollection<VideoTag>? VideoTags { get; set; }
    }
}
