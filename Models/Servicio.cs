using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Servicio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }
        public string? Beneficios { get; set; }
        public string? Caracteristicas { get; set; }

        // Relación muchos a muchos con Proyectos
        public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}