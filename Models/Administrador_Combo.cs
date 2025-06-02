namespace TotemPWA.Models { 
    public class AdministradorCombo { 
        public int AdministradorId { get; set; } 
        public int ComboId { get; set; } 
        public Administrador Administrador { get; set; } 
        public Combo Combo { get; set; } 
    } 
} 
