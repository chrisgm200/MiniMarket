using System.ComponentModel.DataAnnotations;

namespace MiniMarketWebApp.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        // Almacenamos hash (no la contraseña en claro)
        [Required]
        public string PasswordHash { get; set; }

        // Rol: "Administrador" o "Cajero"
        [Required, StringLength(50)]
        public string Rol { get; set; }
    }
}
