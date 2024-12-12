using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace reservsoft.Models
{
    public class Descuento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "El campo IdDescuento es obligatorio.")]
        public int IdDescuento { get; set; }

        [Required(ErrorMessage = "El campo IdApartamento es obligatorio.")]
        [ForeignKey("Apartamentos")]
        public int IdApartamento { get; set; }

        [Required(ErrorMessage = "El campo Descripcion es obligatorio.")]
        [StringLength(250, ErrorMessage = "La descripción no puede tener más de 250 caracteres.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "La descripción solo puede contener letras y espacios.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El campo Descuento es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El descuento debe estar entre 1 y 100.")]
        public int Descuentos { get; set; }

        [NotMapped]
        public double PrecioConDescuento => Math.Round(Precio - (Precio * Descuentos / 100), 2);

        // Propiedades adicionales para mostrar los precios formateados
        [NotMapped]
        public string PrecioFormatted => Precio.ToString("C0", new System.Globalization.CultureInfo("es-CO"));

        [NotMapped]
        public string PrecioConDescuentoFormatted => PrecioConDescuento.ToString("C0", new System.Globalization.CultureInfo("es-CO"));

        [Required(ErrorMessage = "El campo FechaInicio es obligatorio.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo FechaFin es obligatorio.")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio.")]
        public bool Estado { get; set; }

        // Validación adicional para las fechas
        public bool ValidarFechas()
        {
            return FechaInicio <= FechaFin;
        }
    }
}
