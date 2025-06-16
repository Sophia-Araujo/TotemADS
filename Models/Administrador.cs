using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Administrador
    {
        [Key]
        public int AdministradorId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Senha { get; set; }
        [Required]
        public string CPF { get; set; }

        public List<Cupom>? Cupons { get; set; }

        //Coleção de Itens Pedidos
        public List<Produto>? Produto { get; set; }
        //


    }
}
