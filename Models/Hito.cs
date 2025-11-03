using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Hito
    {
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }
        public DateTime? FechaFinalizacionReal { get; set; }

        // Relaciones
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; } = null!;
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}