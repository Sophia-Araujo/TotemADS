using System.Collections.Generic; 
 
namespace TotemPWA.Models { 
    public class Produto { 
        public int ProdutoId { get; set; } 
        public string Nome { get; set; } 
        public string Descricao { get; set; } 
        public double Valor { get; set; } 
        public string Imagem { get; set; } 
        public int CategoriaId { get; set; } 
        public Categoria Categoria { get; set; } 
    } 
} 
