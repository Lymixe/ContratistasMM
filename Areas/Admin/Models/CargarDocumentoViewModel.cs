using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Areas.Admin.Models
{
    public class CargarDocumentoViewModel
    {
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "El nombre del documento es obligatorio.")]
        [Display(Name = "Nombre del Documento")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una clasificación.")]
        public string Clasificacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un archivo.")]
        [Display(Name = "Archivo")]
        public IFormFile Archivo { get; set; }

        [Display(Name = "Visible para el Cliente")]
        public bool EsVisibleParaCliente { get; set; }

    }
}