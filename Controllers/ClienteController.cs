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
    }
 }