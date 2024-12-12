using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace reservsoft.Models
{
    public class Reservas
    {
        [Key]
        public int NumReserva { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        [StringLength(2)]
        public string TipoDoc { get; set; } = string.Empty;

        public string NumDoc { get; set; } = string.Empty;

        [Range(0, 3)]
        public int Acompanantes { get; set; }

        public DateTime FechaReserva { get; set; }

        public DateTime FInicio { get; set; }

        public DateTime FFin { get; set; }

        [Range(0, double.MaxValue)]
        public double TotalReserva { get; set; }

        public EstadoReserva Estado { get; set; }

        public ICollection<Apartamentos> Apartamentos { get; set; } = new List<Apartamentos>();

        [NotMapped]
        public int[] ApartamentoIds { get; set; } = Array.Empty<int>();
    }

    public enum EstadoReserva
    {
        Confirmado,
        Pendiente,
        Cancelado
    }
}
