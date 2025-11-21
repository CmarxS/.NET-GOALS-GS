using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("TB_GOALS_NET")]
    public class Goal
    {
        [Key]
        [Column("id_goal")]
        public int Id { get; set; }

        [Column("id_user")]
        public int? IdUser { get; set; }

        [Required]
        [Column("titulo")]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column("tipo")]
        [MaxLength(12)]
        public string Tipo { get; set; } = "FINANCEIRO"; // FINANCEIRO, HABITO

        [Column("valor_alvo")]
        public decimal? ValorAlvo { get; set; }

        [Column("dias_alvo")]
        public int? DiasAlvo { get; set; }

        [Column("dias_concluidos")]
        public int DiasConcluidos { get; set; } = 0;

        [Column("qtd_alvo_diaria")]
        public decimal? QtdAlvoDiaria { get; set; }

        [Column("unidade")]
        [MaxLength(20)]
        public string? Unidade { get; set; }

        [Column("data_inicio")]
        public DateTime? DataInicio { get; set; }

        [Column("data_fim")]
        public DateTime? DataFim { get; set; }

        [Column("status")]
        [MaxLength(12)]
        public string Status { get; set; } = "ATIVA"; // ATIVA, CONCLUIDA, CANCELADA

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("IdUser")]
        public User? User { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
