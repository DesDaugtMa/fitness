using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public int? UploaderId { get; set; }
        public User? Uploader { get; set; }

        public string? Description { get; set; }

        [Required]
        public required byte[] FileData { get; set; }

        public long FileSizeBytes { get; set; }

        public string? FileExtension { get; set; }

        public string? MimeType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
