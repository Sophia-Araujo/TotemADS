using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Pagamento
    {
        [Key]
        public int PagamentoId { get; set; }
        // entrangeira de Pedido CPF

        //Estrangeira de Pedido
        public string CPF { get; set; }
        public int PedidoId { get; set; }
        public float Valor { get; set; }
        public Pedido Pedido { get; set; }
        //
        public string Tipo { get; set; }
        

    }
}
