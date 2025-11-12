using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class PreguntaFrecuente
    {
        public int Id { get; set; }
        [Required]
        public string Pregunta { get; set; }
        [Required]
        public string Respuesta { get; set; }
        public int Orden { get; set; } = 0;
    }
}