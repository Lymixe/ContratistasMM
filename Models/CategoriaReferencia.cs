using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class CategoriaReferencia
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } // Ej: "Materiales", "Legal"

        public ICollection<RecursoReferencia> Recursos { get; set; } = new List<RecursoReferencia>();
    }
}