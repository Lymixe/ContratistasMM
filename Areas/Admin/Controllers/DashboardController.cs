using ContratistasMM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // ¡MUY IMPORTANTE! Solo los admins pueden acceder
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ProyectosActivos = await _context.Proyectos.CountAsync(p => p.Estado == "En Progreso");
            ViewBag.ProyectosFinalizados = await _context.Proyectos.CountAsync(p => p.Estado == "Finalizado");
            return View();
        }
    }
}