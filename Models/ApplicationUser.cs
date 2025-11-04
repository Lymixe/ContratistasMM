using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ContratistasMM.Models
{
 
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [StringLength(50)]
        public string? Nombre { get; set; }

        [PersonalData]
        [StringLength(50)]
        public string? Apellido { get; set; }
    }
}