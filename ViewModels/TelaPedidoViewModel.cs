using TotemPWA.Models;

namespace TotemPWA.ViewModels
{
    public class TelaPedidosViewModel
    {
        public List<Categoria> Categorias { get; set; } = new List<Categoria>();
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<ProdutoComIngredientes> ProdutosComIngredientes { get; set; } = new List<ProdutoComIngredientes>();
    }

    public class ProdutoComIngredientes
    {
        public Produto Produto { get; set; }
        public List<IngredientePersonalizacao> Ingredientes { get; set; } = new List<IngredientePersonalizacao>();
    }

    public class IngredientePersonalizacao
    {
        public int IgredienteId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }

    }
}