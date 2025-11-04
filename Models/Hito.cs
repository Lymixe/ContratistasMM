using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Hito
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public DateTime? FechaEstimada { get; set; }
        public DateTime? FechaFinalizacionReal { get; set; }

        // Valores: "Pendiente", "En Progreso", "Completado"
        public string Estado { get; set; } = "Pendiente";

        // Relación con Proyecto
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }

        // Relación con ArchivoHito (Un hito puede tener muchos archivos)
        public ICollection<ArchivoHito> ArchivosHito { get; set; } = new List<ArchivoHito>();

        // Relación con Comentario (Un hito puede tener muchos comentarios)
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}