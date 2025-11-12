using ContratistasMM.Models;
using System.Collections.Generic;

namespace ContratistasMM.Areas.Admin.Models
{
    public class GestionReferenciaViewModel
    {
        public List<RecursoReferencia> Recursos { get; set; }
        public List<CategoriaReferencia> Categorias { get; set; }
    }
}