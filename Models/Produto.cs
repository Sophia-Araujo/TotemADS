using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }


        public byte[]? Imagem { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [Display(Name = "Descrição")]


        public string Descricao { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Display(Name = "Valor")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public float Valor { get; set; }

        [Required(ErrorMessage = "Informe se é combo ou não")]
        [Display(Name = "É Combo?")]
        public int IsCombo { get; set; }

        //Lista de Ingredientes de cada Produto
        public List<IgredienteProduto>? IgredienteProdutos { get; set; }

        //Estrangeira de Administrador
        [Required(ErrorMessage = "Selecione um administrador")]
        [Display(Name = "Administrador")]
        public int AdministradorId { get; set; }
        public Administrador? Administrador { get; set; }

        //Estrangeira de Categoria
        [Required(ErrorMessage = "Selecione uma categoria")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        //Coleção de Itens Pedidos
        public List<ItensPedido>? ItensPedidos { get; set; }

        //Coleção de Itens Combo
        public List<ItensCombo>? ItensCombo { get; set; }

        public virtual ICollection<Cupom>? Cupons { get; set; }

    }

}