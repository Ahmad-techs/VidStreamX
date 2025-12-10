using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidStreamX.Data;
using VidStreamX.Models;

namespace VidStreamX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public VideoController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ---------------- Upload Video ----------------
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo([FromForm] VideoUploadDto dto)
        {
            // Validate User exists
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found");

            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");

            // Save file to wwwroot/uploads
            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await dto.File.CopyToAsync(stream);
            }

            var video = new Video
            {
                Title = dto.Title,
                FileName = dto.File.FileName,
                BlobName = uniqueFileName,
                BlobUrl = $"/uploads/{uniqueFileName}",
                UploadedAt = DateTime.UtcNow,
                OwnerId = user.Id
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                video.Id,
                video.Title,
                BlobUrl = $"{Request.Scheme}://{Request.Host}{video.BlobUrl}" // full URL
            });
        }

        // ---------------- Get All Videos ----------------
        [HttpGet("feed")]
        public async Task<IActionResult> GetVideos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var videos = await _context.Videos
                .Include(v => v.Owner)
                .Include(v => v.Likes)
                .OrderByDescending(v => v.UploadedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new
                {
                    v.Id,
                    v.Title,
                    BlobUrl = $"{Request.Scheme}://{Request.Host}{v.BlobUrl}",
                    Owner = v.Owner.Username,
                    LikesCount = v.Likes.Count
                })
                .ToListAsync();

            return Ok(videos);
        }

        // ---------------- Like Video ----------------

        [Authorize]
        [HttpPost("{videoId}/like")]
        public async Task<IActionResult> LikeVideo(Guid videoId, [FromBody] LikeDto dto)
        {
            // Validate User
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found");

            // Validate Video
            var video = await _context.Videos.Include(v => v.Likes)
                                             .FirstOrDefaultAsync(v => v.Id == videoId);
            if (video == null)
                return NotFound("Video not found");

            // Check if already liked
            if (video.Likes.Any(l => l.UserId == user.Id))
                return BadRequest("User already liked this video");

            var like = new Like
            {
                VideoId = video.Id,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            // Reload like count from database
            var likesCount = await _context.Likes.CountAsync(l => l.VideoId == video.Id);

            return Ok(new { Message = "Video liked", LikesCount = likesCount });
        }
    }

        // ---------------- DTOs ----------------
        public class VideoUploadDto
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }

    public class LikeDto
    {
        public Guid UserId { get; set; }
    }
}