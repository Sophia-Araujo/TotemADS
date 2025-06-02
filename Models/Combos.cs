namespace TotemPWA.Models {
    public class Combo
    {
        public int ComboId { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }

        public ICollection<Cupom> Cupoms { get; set; } = new List<Cupom>();
    } 
} 
