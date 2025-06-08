using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required]
        public byte[] Imagem { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public float Valor { get; set; }
        [Required]
        public int IsCombo { get; set; }


        //Lista de Igredientes de cada Produto
        public List<IgredienteProduto> IgredienteProdutos { get; set; }
        //

        //Estrangeira de Administrador
        [Required]
        public int AdministradorId { get; set; }
        public Administrador Administrador { get; set; }
        //

        //Estrangeira de Categoria
        [Required]
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
        //


        //Coleção de Itens Pedidos
        public List<ItensPedido> ItensPedidos { get; set; }
        //
    }
}
