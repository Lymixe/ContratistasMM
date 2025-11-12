using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContratistasMM.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComentariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Comentarios
        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Autor) // Incluye los datos del autor para mostrar su nombre
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
            return View(comentarios);
        }

        // POST: /Comentarios/Crear
        [HttpPost]
        [Authorize] // Solo usuarios autenticados pueden comentar
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(string contenido)
        {
            if (!string.IsNullOrWhiteSpace(contenido))
            {
                var autorId = _userManager.GetUserId(User);
                var comentario = new Comentario
                {
                    Contenido = contenido,
                    AutorId = autorId,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Comentarios.Add(comentario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Comentarios/Eliminar/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // Solo el Admin puede eliminar
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}