using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("TB_TRANSACTIONS_NET")]
    public class Transaction
    {
  [Key]
        [Column("id_transaction")]
    public int Id { get; set; }

        [Required]
   [Column("id_user")]
  public int IdUser { get; set; }

   [Required]
   [Column("id_category")]
        public int IdCategory { get; set; }

        [Column("id_goal")]
 public int? IdGoal { get; set; }

        [Required]
     [Column("tipo")]
        [MaxLength(12)]
        public string Tipo { get; set; } = string.Empty; // DESPESA, RECEITA

        [Required]
   [Column("valor")]
        public decimal Valor { get; set; }

        [Column("descricao")]
  [MaxLength(200)]
        public string? Descricao { get; set; }

   [Column("merchant")]
  [MaxLength(100)]
        public string? Merchant { get; set; }

        [Required]
   [Column("data_transacao")]
        public DateTime DataTransacao { get; set; }

   [Column("created_at")]
  public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
 [ForeignKey("IdUser")]
        public User? User { get; set; }

     [ForeignKey("IdCategory")]
   public Category? Category { get; set; }

   [ForeignKey("IdGoal")]
  public Goal? Goal { get; set; }
    }
}
