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
        public string TipoRecurso { get; set; }

        public string? Url { get; set; }

        // Relaciones
        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaReferenciaId { get; set; }
        public CategoriaReferencia CategoriaReferencia { get; set; }
    }
}