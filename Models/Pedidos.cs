using System.Text.Json.Serialization;
using TotemPWA.Models; // Adicione isso

namespace TotemPWA.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public double Valor { get; set; }
        public int IdTransacao { get; set; }
        public required string CPF { get; set; }

        public int ClienteId { get; set; }

      
        public required Cliente Cliente { get; set; }

      
        public required Pagamento Pagamento { get; set; }

        // Relacionamento inexistentes no DER (lado do 1)

        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
