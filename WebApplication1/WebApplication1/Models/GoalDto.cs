namespace WebApplication1.Models
{
    public class GoalDto
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    public decimal? ValorAlvo { get; set; }
        public int? DiasAlvo { get; set; }
   public int DiasConcluidos { get; set; }
     public decimal? QtdAlvoDiaria { get; set; }
  public string? Unidade { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; } = string.Empty;
   public DateTime CreatedAt { get; set; }
     
        // Info relacionadas
     public string? UserNome { get; set; }
        
  public Dictionary<string, string> Links { get; set; } = new();
    }

    public class CreateGoalDto
    {
        public int? IdUser { get; set; }
        public string Titulo { get; set; } = string.Empty;
   public string Tipo { get; set; } = "FINANCEIRO"; // FINANCEIRO, HABITO
     public decimal? ValorAlvo { get; set; }
        public int? DiasAlvo { get; set; }
   public decimal? QtdAlvoDiaria { get; set; }
        public string? Unidade { get; set; }
     public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
 }

    public class UpdateGoalDto
    {
 public string? Titulo { get; set; }
   public string? Tipo { get; set; }
        public decimal? ValorAlvo { get; set; }
        public int? DiasAlvo { get; set; }
        public int? DiasConcluidos { get; set; }
    public decimal? QtdAlvoDiaria { get; set; }
        public string? Unidade { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Status { get; set; }
    }
}
