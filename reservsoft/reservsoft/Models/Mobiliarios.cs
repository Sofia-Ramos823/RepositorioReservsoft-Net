using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reservsoft.Models
{
    public enum EstadoMobiliario
    {
        Activo,
        Inactivo,
        Mantenimiento,
        Transferido
    }
    public class Mobiliario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "El campo IdMobiliario es obligatorio.")]
        public int IdMobiliario { get; set; }

        [Required(ErrorMessage = "El campo IdApartamento es obligatorio.")]
        [ForeignKey("Apartamentos")]
        public int IdApartamento { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo IdentMobiliario es obligatorio.")]
        [StringLength(100, ErrorMessage = "El IdentMobiliario no puede tener más de 100 caracteres.")]
        public string IdentMobiliario { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio.")]
        public EstadoMobiliario Estado { get; set; }

        [StringLength(500, ErrorMessage = "La observación no puede tener más de 500 caracteres.")]
        public string Observacion { get; set; }
    }
}