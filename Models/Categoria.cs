namespace TotemPWA.Models {
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public int? Parente { get; set; }

        public int ProdutoId { get; set; }
        
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    } 
} 
