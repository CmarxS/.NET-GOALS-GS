using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("TB_CATEGORIES_NET")]
 public class Category
    {
        [Key]
        [Column("id_category")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

     [Required]
   [Column("tipo")]
     [MaxLength(20)]
        public string Tipo { get; set; } = string.Empty; // DESPESA, RECEITA

        [Column("limite_mensal")]
     public decimal? LimiteMensal { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

   // Navigation properties
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
