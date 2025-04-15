using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Motosfera.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
