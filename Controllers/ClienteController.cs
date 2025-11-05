using ContratistasMM.Areas.Admin.Models;
using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClienteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Muestra los proyectos asignados al cliente que ha iniciado sesión
        public async Task<IActionResult> MisProyectos()
        {
            var userId = _userManager.GetUserId(User);
            var proyectosCliente = await _context.Proyectos
                                                 .Where(p => p.ClienteId == userId)
                                                 .OrderByDescending(p => p.Id)
                                                 .ToListAsync();
            return View(proyectosCliente);
        }
        public async Task<IActionResult> DetalleProyecto(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var proyecto = await _context.Proyectos
                .Include(p => p.Hitos.OrderBy(h => h.FechaEstimada))
                    .ThenInclude(h => h.ArchivosHito)
                .Include(p => p.Documentos.Where(d => d.EsVisibleParaCliente)) // ¡SOLO DOCUMENTOS VISIBLES!
                .FirstOrDefaultAsync(p => p.Id == id && p.ClienteId == userId); // Doble seguridad

            if (proyecto == null)
            {
                // El proyecto no existe o no pertenece a este cliente
                return Forbid();
            }

            // Usaremos el mismo ViewModel del Admin, ya que contiene toda la data.
            var viewModel = new DetalleProyectoViewModel
            {
                Proyecto = proyecto
            };
            
            return View(viewModel);
        }
    }
 }