using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequesterId { get; set; }
        public User Requester { get; set; } = null!;

        [Required]
        public int AddresseeId { get; set; }
        public User Addressee { get; set; } = null!;

        [Required]
        public required string Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
