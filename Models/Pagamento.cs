using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Pagamento
    {
        [Key]
        public int PagamentoId { get; set; }
        // entrangeira de Pedido CPF

        //Estrangeira de Pedido
        [Required]
        public string CPF { get; set; }
        [Required]
        public int PedidoId { get; set; }
        [Required]
        public float Valor { get; set; }
        public Pedido Pedido { get; set; }
        //
        [Required]
        public string Tipo { get; set; }
        

    }
}
