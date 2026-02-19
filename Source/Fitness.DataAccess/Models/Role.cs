using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
