using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public int? CategoriaPaiId { get; set; } // Pode ser nulo
        public Categoria? CategoriaPai { get; set; } // Navegação para pai
        public ICollection<Categoria>? Subcategorias { get; set; } // Navegação para filhos

        public List<Produto>? Produtos { get; set; }
    }
}