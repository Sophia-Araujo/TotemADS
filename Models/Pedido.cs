using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }

        public float Valor { get; set; }


        //Estrangeiras de Pagamentos
        public int PagamentoId { get; set; }
        public Pagamento Pagamento { get; set; }
        //


        //Estrangeiras de Cliente
        public int ClienteId { get; set; }
        public string CPF { get; set; }
        public Cliente Cliente { get; set; }
        //

        //Coleção de Itens Pedidos
        public List<ItensPedido> ItensPedido { get; set; }

        //
    }
}
