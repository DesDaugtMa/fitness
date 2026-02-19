using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Email { get; set; }

        public string? PasswordHash { get; set; }

        [Required]
        public required string DisplayName { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int? ProfileImageId { get; set; }
        public Image? ProfileImage { get; set; }

        public string? AuthProvider { get; set; }
        public string? ProviderId { get; set; }

        public bool ShareExercisesEnabled { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Friendship> FriendshipsRequested { get; set; } = new List<Friendship>();
        public ICollection<Friendship> FriendshipsReceived { get; set; } = new List<Friendship>();
    }
}
