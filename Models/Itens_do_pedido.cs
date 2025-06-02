namespace TotemPWA.Models
{
    public class ItemPedido
    {
        public int itemId { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }
    }
}
