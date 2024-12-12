using reservsoft.Models; // Espacio de nombres del proyecto
using System.ComponentModel.DataAnnotations; // Atributos de validación
using System.ComponentModel.DataAnnotations.Schema; // Atributos de base de datos
using System.Collections.Generic; // Para ICollection

namespace reservsoft.Models
{
    public class Apartamentos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "El campo IdApartamento es obligatorio.")]
        public int IdApartamento { get; set; }

        [Required(ErrorMessage = "El campo TipoApartamento es obligatorio.")]
        [StringLength(80, ErrorMessage = "El campo TipoApartamento no puede tener más de 80 caracteres.")]
        public string TipoApartamento { get; set; }

        [Required(ErrorMessage = "El campo Descripcion es obligatorio.")]
        [StringLength(250, ErrorMessage = "El campo Descripcion no puede tener más de 250 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Capacidad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Capacidad debe ser mayor a cero.")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El campo Tamaño es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo Tamaño no puede tener más de 50 caracteres.")]
        public string Tamaño { get; set; }

        [Required(ErrorMessage = "El campo Piso es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo Piso debe ser un valor mayor a cero.")]
        public int Piso { get; set; }

        [Required(ErrorMessage = "El campo Tarifa es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo Tarifa debe ser mayor a cero.")]
        public double Tarifa { get; set; }

        [NotMapped]
        public string TarifaFormatted
        {
            get
            {
                return Tarifa.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
            }
        }

        public ICollection<Reservas> Reserva { get; set; } = new List<Reservas>();
    }
}
