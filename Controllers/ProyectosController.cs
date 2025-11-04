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
            ViewData["BusquedaActual"] = busqueda;

            var proyectosQuery = _context.Proyectos.Where(p => p.EsPublico);

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

            var proyectos = await proyectosQuery.OrderByDescending(p => p.AnioEjecucion).ToListAsync();

            return View(proyectos);
        }

        // ACCIÓN PARA MOSTRAR EL DETALLE DE UN PROYECTO (CON LÓGICA DE REGRESO)
        public async Task<IActionResult> Detalle(int? id, string origen, int? servicioId)
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

            ViewBag.Origen = origen;
            ViewBag.ServicioId = servicioId;

            return View(proyecto);
        }
    }
}