using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("TB_USERS_NET")]
    public class User
    {
        [Key]
      [Column("id_user")]
     public int Id { get; set; }

        [Required]
   [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Column("email")]
    [MaxLength(120)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("senha_hash")]
        [MaxLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        [Column("role")]
        [MaxLength(20)]
        public string Role { get; set; } = "USER"; // USER, ADMIN

 [Column("created_at")]
   public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
   public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
