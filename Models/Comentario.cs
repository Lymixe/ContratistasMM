using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string Contenido { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public int HitoId { get; set; }
        public Hito Hito { get; set; } = null!;
        public string AutorId { get; set; }
        public ApplicationUser Autor { get; set; } = null!;
    }
}