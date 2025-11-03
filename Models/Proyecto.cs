using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Proyecto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string? Ubicacion { get; set; }
        public int? AnioEjecucion { get; set; }
        public string? TipoObra { get; set; }
        public string? ImagenUrl { get; set; } // Imagen principal
        public bool EsPublico { get; set; } = false; // Para HU-05, controlar si es visible en el portafolio

        // Relaciones
        public string? ClienteId { get; set; }
        public ApplicationUser? Cliente { get; set; }
        public ICollection<Hito> Hitos { get; set; } = new List<Hito>();
        public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}