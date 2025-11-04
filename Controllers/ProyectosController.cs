using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ContratistasMM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProyectosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ACCIÓN PARA MOSTRAR LA GALERÍA DE PROYECTOS
        // Responde a la URL: /Proyectos
        public async Task<IActionResult> Index()
        {
            // Tarea HU01-T4: Consulta solo los proyectos marcados como públicos (EsPublico = true)
            var proyectos = await _context.Proyectos
                                          .Where(p => p.EsPublico)
                                          .OrderByDescending(p => p.AnioEjecucion) // Ordena por año, del más reciente al más antiguo
                                          .ToListAsync();
            return View(proyectos);
        }

        // ACCIÓN PARA MOSTRAR EL DETALLE DE UN PROYECTO
        // Responde a URLs como: /Proyectos/Detalle/1
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Si no se provee un ID, no se encuentra
            }

            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id);

            if (proyecto == null || !proyecto.EsPublico)
            {
                // Si el proyecto no existe o no es público, se trata como no encontrado
                return NotFound();
            }

            return View(proyecto);
        }
    }
}