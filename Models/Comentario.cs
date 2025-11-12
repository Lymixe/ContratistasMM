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
        [MaxLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres.")]
        public string Contenido { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con el autor (usuario que lo escribió)
        [Required]
        public string AutorId { get; set; }
        public ApplicationUser Autor { get; set; } = null!;
    }
}