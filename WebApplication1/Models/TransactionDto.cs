namespace WebApplication1.Models
{
    public class TransactionDto
    {
        public int Id { get; set; }
 public int IdUser { get; set; }
   public int IdCategory { get; set; }
        public int? IdGoal { get; set; }
  public string Tipo { get; set; } = string.Empty;
     public decimal Valor { get; set; }
     public string? Descricao { get; set; }
public string? Merchant { get; set; }
        public DateTime DataTransacao { get; set; }
        public DateTime CreatedAt { get; set; }
  
        // Info relacionadas
        public string? UserNome { get; set; }
        public string? CategoryNome { get; set; }
        public string? GoalTitulo { get; set; }
      
      public Dictionary<string, string> Links { get; set; } = new();
 }

    public class CreateTransactionDto
    {
 public int IdUser { get; set; }
    public int IdCategory { get; set; }
   public int? IdGoal { get; set; }
        public string Tipo { get; set; } = string.Empty; // DESPESA, RECEITA
        public decimal Valor { get; set; }
     public string? Descricao { get; set; }
  public string? Merchant { get; set; }
        public DateTime DataTransacao { get; set; }
    }

    public class UpdateTransactionDto
    {
     public int? IdCategory { get; set; }
   public int? IdGoal { get; set; }
 public string? Tipo { get; set; }
  public decimal? Valor { get; set; }
  public string? Descricao { get; set; }
        public string? Merchant { get; set; }
   public DateTime? DataTransacao { get; set; }
 }
}
