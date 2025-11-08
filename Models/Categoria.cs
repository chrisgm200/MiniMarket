using System.ComponentModel.DataAnnotations;

namespace MiniMarketWebApp.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }
    }
}
