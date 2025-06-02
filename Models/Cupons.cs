namespace TotemPWA.Models {
    public class Cupom
    {
        public int CupomId { get; set; }
        public string Codigo { get; set; }
        public double Desconto { get; set; }
        public int ComboId { get; set; }
        public string Validade { get; set; }
        public Combo Combo { get; set; } 

    } 
} 
