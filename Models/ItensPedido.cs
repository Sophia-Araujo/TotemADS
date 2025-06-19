using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class ItensPedido
    {
        [Key]
        public int ItensPedidoId { get; set; }

        //Navegação de Pedido
        public Pedido Pedido { get; set; }
        public int PedidoId { get; set; }
        //

        //Navegação de Produto
        public Produto Produto { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        //
        [Required]
        public int Quantidade { get; set; }

        //Lista de Adicionais
        public List<Adicional>? Adicionais { get; set; }
        //
        

        

    }
}
