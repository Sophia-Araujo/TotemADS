using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Igrediente
    {
        [Key]
        public int IgredienteId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public byte[]? Imagem { get; set; }
        [Required]
        public float Valor { get; set; }

        //lista de igradientes de produtos
        public List<IgredienteProduto>? IgredienteProdutos { get; set; }
        //


        //lista de adicionais
        public List<Adicional>? Adicionais { get; set; }
        //
    }
}