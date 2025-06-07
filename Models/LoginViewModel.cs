// Caminho esperado: SeuProjeto/Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations; // Necessário para [Required]

namespace TotemPWA.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        [Display(Name = "Usuário")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)] // Esconde a entrada da senha
        [Display(Name = "Senha")]
        public string Password { get; set; }
    }
}