using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotemPWA.Models
{
    [Table("Cupons")]
    public class Cupom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CupomId { get; set; }

        [Required(ErrorMessage = "O código do cupom é obrigatório")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O código deve ter entre 1 e 50 caracteres")]
        [Display(Name = "Código do Cupom")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O desconto é obrigatório")]
        [Range(0.01, 100.00, ErrorMessage = "O desconto deve estar entre 0.01% e 100%")]
        [Display(Name = "Desconto (%)")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Desconto { get; set; }

        [Required(ErrorMessage = "A data de validade é obrigatória")]
        [Display(Name = "Data de Validade")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Validade { get; set; }


        [Required(ErrorMessage = "O administrador é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um administrador válido")]
        [Display(Name = "Administrador")]
        public int AdministradorId { get; set; }

        // Propriedades de navegação
        [ForeignKey("AdministradorId")]
        public virtual Administrador? Administrador { get; set; }
        // Método para verificar se o cupom está válido
        public bool IsValido()
        {
            return Validade >= DateTime.Now.Date;
        }

        // Método para calcular o desconto
        public decimal CalcularDesconto(decimal valorOriginal)
        {
            if (!IsValido()) return 0;
            return valorOriginal * (Desconto / 100);
        }

        public int? ProdutoId { get; set; } // Nullable, pois o produto é opcional
        public Produto? Produto { get; set; } // Navegação

    }
}