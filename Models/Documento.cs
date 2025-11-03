using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Documento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } // El nombre amigable del archivo, ej: "Planos Eléctricos"

        [Required]
        public string UrlArchivo { get; set; } // La ruta donde se guarda el archivo

        [Required]
        [StringLength(50)]
        public string Clasificacion { get; set; } // El tipo de documento, ej: "Recibo", "Permiso", "Plano"

        // Relaciones
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; } = null!;
    }
}