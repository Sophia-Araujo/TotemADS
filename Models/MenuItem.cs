using System.Collections.Generic;

namespace TotemPWA.Models
{
    public class MenuItem
    {
        public string Nome { get; set; }
        public string Icone { get; set; } // Opcional
        public string Link { get; set; }
        public List<MenuItem> SubMenuItems { get; set; } // Essencial para submenus futuros
    }
}