using Microsoft.EntityFrameworkCore;
using VidStreamX.Models;

namespace VidStreamX.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Video> Videos { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<VideoTag> VideoTags { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- User ----------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Use Username and Email (these exist in your model)
                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();

                // Navigation collections are inferred (Videos, Likes, Comments)
            });

            // ---------- Video ----------
            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.FileName)
                      .IsRequired()
                      .HasMaxLength(260);

                entity.Property(e => e.BlobName)
                      .IsRequired()
                      .HasMaxLength(260);

                entity.Property(e => e.BlobUrl)
                      .HasMaxLength(1000);

                // Owner relationship: Video.Owner (OwnerId in Video model)
                entity.HasOne(v => v.Owner)
                      .WithMany(u => u.Videos)
                      .HasForeignKey(v => v.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Indexes for common queries
                entity.HasIndex(v => v.UploadedAt);
                entity.HasIndex(v => v.OwnerId);
            });

            // ---------- Like ----------
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(l => l.User)
                      .WithMany(u => u.Likes)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(l => l.Video)
                      .WithMany(v => v.Likes)
                      .HasForeignKey(l => l.VideoId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure each user can like a video only once (unique index)
                entity.HasIndex(l => new { l.UserId, l.VideoId }).IsUnique();
            });

            // ---------- Comment ----------
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Text).IsRequired().HasMaxLength(2000);

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Video)
                      .WithMany(v => v.Comments)
                      .HasForeignKey(c => c.VideoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(c => c.VideoId);
            });

            // ---------- Tag ----------
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // ---------- VideoTag (many-to-many) ----------
            modelBuilder.Entity<VideoTag>(entity =>
            {
                // configure composite key
                entity.HasKey(vt => new { vt.VideoId, vt.TagId });

                entity.HasOne(vt => vt.Video)
                      .WithMany(v => v.VideoTags)
                      .HasForeignKey(vt => vt.VideoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(vt => vt.Tag)
                      .WithMany(t => t.VideoTags)
                      .HasForeignKey(vt => vt.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- RefreshToken ----------
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.Token).IsUnique();
            });
        }
    }
}
