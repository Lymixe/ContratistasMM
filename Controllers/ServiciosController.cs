using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContratistasMM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ACCIÓN PARA MOSTRAR EL CATÁLOGO DE SERVICIOS
        // URL: /Servicios
        public async Task<IActionResult> Index()
        {
            var servicios = await _context.Servicios.ToListAsync();
            return View(servicios);
        }

        // ACCIÓN PARA MOSTRAR EL DETALLE DE UN SERVICIO
        // URL: /Servicios/Detalle/1
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Usamos .Include() para traer también los proyectos asociados a este servicio
            var servicio = await _context.Servicios
                                         .Include(s => s.Proyectos)
                                         .FirstOrDefaultAsync(s => s.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }
    }
}