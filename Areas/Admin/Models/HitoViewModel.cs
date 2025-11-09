using System.ComponentModel.DataAnnotations;
using ContratistasMM.Models;

namespace ContratistasMM.Areas.Admin.Models
{
    public class HitoViewModel
    {
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "El nombre del hito es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha Estimada")]
        [DataType(DataType.Date)]
        public DateTime? FechaEstimada { get; set; }

        [Required]
        public string Estado { get; set; } = "Pendiente";

        // Para subir múltiples archivos
        public List<IFormFile> Archivos { get; set; } = new();

        public List<int> PersonalIds { get; set; } = new();
        public List<Personal> PersonalDisponible { get; set; } = new();
    }
}