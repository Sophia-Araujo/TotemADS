using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class IgredienteProduto
    {
        [Key]
        public int IgredienteProdutoId { get; set; }

        // Estrangeira de Igrediente
        [Required]
        public int IgredienteId { get; set; }
        public Igrediente Igrediente { get; set; }

        // Estrangeira de Produto
        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        // Quantidade do ingrediente no produto
        [Required]
        public float Quantidade { get; set; }
    }
    
}