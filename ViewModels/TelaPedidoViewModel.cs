using TotemPWA.Models;

namespace TotemPWA.ViewModels
{
    public class TelaPedidosViewModel
    {
        public List<Categoria> Categorias { get; set; } = new List<Categoria>();
        public List<Produto> Produtos { get; set; } = new List<Produto>();
    }
}