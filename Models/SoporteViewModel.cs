using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContratistasMM.Models
{
    public class SoporteViewModel
    {
        public List<RecursoReferencia> Recursos { get; set; }
        public List<CategoriaReferencia> Categorias { get; set; }
        public List<PreguntaFrecuente> FAQs { get; set; }
    }
}