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

        // ACCIÓN PARA MOSTRAR LA GALERÍA DE PROYECTOS (CON BÚSQUEDA)
        public async Task<IActionResult> Index(string busqueda)
        {
            // Pasamos el término de búsqueda a la vista para que el input lo recuerde
            ViewData["BusquedaActual"] = busqueda;

            // Empezamos con la consulta base: solo proyectos públicos
            var proyectosQuery = _context.Proyectos.Where(p => p.EsPublico);

            // Si el término de búsqueda no está vacío, filtramos la consulta
            if (!String.IsNullOrEmpty(busqueda))
            {
                string busquedaLower = busqueda.ToLower();
                proyectosQuery = proyectosQuery.Where(p =>
                    p.Nombre.ToLower().Contains(busquedaLower) ||
                    p.Descripcion.ToLower().Contains(busquedaLower) ||
                    p.TipoObra.ToLower().Contains(busquedaLower) ||
                    p.Ubicacion.ToLower().Contains(busquedaLower)
                );
            }

            // Finalmente, ordenamos y ejecutamos la consulta
            var proyectos = await proyectosQuery.OrderByDescending(p => p.AnioEjecucion).ToListAsync();

            return View(proyectos);
        }

        // ACCIÓN PARA MOSTRAR EL DETALLE DE UN PROYECTO
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id);

            if (proyecto == null || !proyecto.EsPublico)
            {
                return NotFound();
            }

            return View(proyecto);
        }
    }
}