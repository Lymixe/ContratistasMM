using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FaqController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaqController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var faqs = await _context.PreguntasFrecuentes.OrderBy(f => f.Orden).ToListAsync();
            return View(faqs);
        }

        public IActionResult Crear()
        {
            return PartialView("_CrearEditarFaqModal", new PreguntaFrecuente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(PreguntaFrecuente faq)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faq);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pregunta frecuente creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al crear la pregunta.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var faq = await _context.PreguntasFrecuentes.FindAsync(id);
            if (faq == null) return NotFound();
            return PartialView("_CrearEditarFaqModal", faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, PreguntaFrecuente faq)
        {
            if (id != faq.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(faq);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pregunta frecuente actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al actualizar la pregunta.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var faq = await _context.PreguntasFrecuentes.FindAsync(id);
            if (faq != null)
            {
                _context.PreguntasFrecuentes.Remove(faq);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pregunta frecuente eliminada exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}