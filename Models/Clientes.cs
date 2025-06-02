using System.Collections.Generic; 
 
namespace TotemPWA.Models {
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public int Pontos { get; set; } 
        
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    } 
} 
