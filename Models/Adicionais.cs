namespace TotemPWA.Models { 
    public class Adicional { 
        public int ProdutoId { get; set; } 
        public int IngredienteId { get; set; } 
        public int PedidoId { get; set; } 
        public int Quantidade { get; set; } 
        public Produto Produto { get; set; } 
        public Ingrediente Ingrediente { get; set; } 
        public Pedido Pedido { get; set; } 
    } 
} 
