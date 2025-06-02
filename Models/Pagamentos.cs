namespace TotemPWA.Models { 
    public class Pagamento { 
        public int PagamentoId { get; set; } 
        public string CPF { get; set; } 
        public double Valor { get; set; } 
        public int clienteId { get; set;  }
        public Cliente Cliente { get; set; } 
    } 
} 
