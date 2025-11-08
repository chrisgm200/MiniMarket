using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniMarketWebApp.Models
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // Relación: una venta tiene varios detalles
        public ICollection<VentaDetalle> Detalles { get; set; }
    }
}
