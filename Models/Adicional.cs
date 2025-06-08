using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Adicional
    {
        [Key]
        public int AdicionalId { get; set; }
        [Required]
        public int Quantidade { get; set; }

        //Estrtangeira de Igredientes
        [Required]
        public int IgredienteId { get; set; }
        public Igrediente Igrediente { get; set; }
        //

        //Estrangeira de ItensPedido
        [Required]
        public int ItensPedidoId { get; set; }
        public ItensPedido ItensPedido { get; set; }
        //
        

    }
}