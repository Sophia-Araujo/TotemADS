namespace TotemPWA.Models { 
    public class AdministradorProduto { 
        public int AdministradorId { get; set; } 
        public int ProdutoId { get; set; } 
        public Administrador Administrador { get; set; } 
        public Produto Produto { get; set; } 
    } 
} 
