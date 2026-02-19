using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class RegistrationToken
    {
        public required string Token { get; set; }

        public int? CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public int? UsedByUserId { get; set; }
        public User? UsedByUser { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
    }
}
