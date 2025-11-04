using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class ArchivoHito
    {
        public int Id { get; set; }

        [Required]
        public string UrlArchivo { get; set; }

        // Tipo: "Imagen", "Video"
        [Required]
        public string Tipo { get; set; }

        // Relación con Hito
        public int HitoId { get; set; }
        public Hito Hito { get; set; }
    }
}