using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class ItensCombo
    {
        [Key]
        public int ItensComboId { get; set; }

        [Required]
        public int Quantidade { get; set; }
        [Required]
        public int ComboId { get; set; }

        //Estrangeira de Produto
        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        //

        //Estrangeira de Cupom
        public Cupom Cupom { get; set; }
        public int? CupomId { get; set; }
        //
    }
}