namespace TotemPWA.Models { 
    public class ItemCombo { 
        public int ComboId { get; set; } 
        public int ProdutoId { get; set; } 
        public int Quantidade { get; set; } 
        public Combo Combo { get; set; } 
        public Produto Produto { get; set; } 
    } 
} 
