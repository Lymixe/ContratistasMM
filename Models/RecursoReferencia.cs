using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class RecursoReferencia
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }

        [Required]
        public string Url { get; set; } // Puede ser un PDF o un enlace a un video

        // Relaciones
        public int CategoriaReferenciaId { get; set; }
        public CategoriaReferencia CategoriaReferencia { get; set; } = null!;
    }
}