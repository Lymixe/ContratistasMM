using ContratistasMM.Models;

namespace ContratistasMM.Areas.Admin.Models
{
    public class DetalleProyectoViewModel
    {
        public Proyecto Proyecto { get; set; }
        // Podríamos necesitar ViewModels específicos para Hito y Documento en el futuro
    }
}