using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
    public class Personal
    {
        public int Id { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Cargo { get; set; } 
        public string? FotoUrl { get; set; }
        public string Estado { get; set; } = "Activo"; 

        public ICollection<Hito> HitosAsignados { get; set; } = new List<Hito>();
    }
}