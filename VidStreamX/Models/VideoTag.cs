using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VidStreamX.Models;

namespace VidStreamX.Models
{
    public class VideoTag
    {
        // Composite key will be configured in DbContext Fluent API
        public Guid VideoId { get; set; }
        [ForeignKey(nameof(VideoId))]
        public Video? Video { get; set; }

        public Guid TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public Tag? Tag { get; set; }
    }
}
