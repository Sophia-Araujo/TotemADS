using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Cupom
    {
        [Key]
        public int CupomId { get; set; }
        [Required]
        public String Codigo { get; set; }
        [Required]
        public float Desconto { get; set; }
        [Required]
        public DateTime Validade { get; set; }
        
        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        //Etrangeira de Administrador
        [Required]
        public int AdministradorId { get; set; }
        public Administrador Administrador { get; set; }
        //

        // Coleção de ItensCombo
        public ItensCombo ItensCombo { get; set; }
        //



    }
}
