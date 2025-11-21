namespace WebApplication1.Models
{
    public class CategoryDto
    {
  public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
   public decimal? LimiteMensal { get; set; }
        public DateTime CreatedAt { get; set; }
    public Dictionary<string, string> Links { get; set; } = new();
    }

    public class CreateCategoryDto
    {
        public string Nome { get; set; } = string.Empty;
   public string Tipo { get; set; } = string.Empty; // DESPESA, RECEITA
        public decimal? LimiteMensal { get; set; }
    }

    public class UpdateCategoryDto
    {
   public string? Nome { get; set; }
   public string? Tipo { get; set; }
        public decimal? LimiteMensal { get; set; }
    }
}
