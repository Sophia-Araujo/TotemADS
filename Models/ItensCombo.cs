using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class ItensCombo
    {
        [Key]
        public int ItensComboId { get; set; }

        [Required]
        public int Quantidade { get; set; }

        //Estrangeira de Produto
        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        //

        //Estrangeira de Cupom
        public Cupom Cupom { get; set; }
        [Required]
        public int? CupomId { get; set; }
        //
    }
}