using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Nome { get; set; }
        public int? Pontos { get; set; }

        public List<Pedido>? Pedidos { get; set; }

    }
}
