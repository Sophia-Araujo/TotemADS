using System.ComponentModel.DataAnnotations;

namespace TotemPWA.Models
{
    public class Igrediente
    {
        [Key]
        public int IgredienteId { get; set; }   
        public string Nome { get; set; }    
        public byte[] Imagem { get; set; }
        public float Valor { get; set; }    

        public List<Adicional> Adicionais { get; set; }
    }
}