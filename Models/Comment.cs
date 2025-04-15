using System.ComponentModel.DataAnnotations;

namespace Motosfera.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Content { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
