using ContratistasMM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Areas.Admin.Models
{
    public class ProyectoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }
        
        public string? Ubicacion { get; set; }

        [Display(Name = "Año de Ejecución")]
        public int? AnioEjecucion { get; set; }

        [Display(Name = "Tipo de Obra")]
        public string? TipoObra { get; set; }

        public string Estado { get; set; }

        [Range(0, 100, ErrorMessage = "El progreso debe estar entre 0 y 100.")]
        public int Progreso { get; set; }

        [Display(Name = "Visible en Portafolio Público")]
        public bool EsPublico { get; set; }

        // Para la lista desplegable de clientes
        [Display(Name = "Cliente Asignado")]
        public string? SelectedClienteId { get; set; }
        public IEnumerable<SelectListItem>? Clientes { get; set; }
    }
}