using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PersonalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PersonalController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Admin/Personal
        public async Task<IActionResult> Index()
        {
            var personal = await _context.Personal.OrderBy(p => p.NombreCompleto).ToListAsync();
            return View(personal);
        }

        // GET: Muestra el modal para crear
        public IActionResult Crear()
        {
            return PartialView("_CrearEditarPersonalModal", new Personal());
        }

        // POST: Procesa la creación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Personal personal, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/personal");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                    }
                    personal.FotoUrl = $"/img/personal/{uniqueFileName}";
                }

                _context.Add(personal);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Miembro del personal creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al crear al miembro del personal.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Muestra el modal para editar
        public async Task<IActionResult> Editar(int id)
        {
            var persona = await _context.Personal.FindAsync(id);
            if (persona == null) return NotFound();
            return PartialView("_CrearEditarPersonalModal", persona);
        }

        // POST: Procesa la edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Personal personal, IFormFile? foto)
        {
            if (id != personal.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var personaToUpdate = await _context.Personal.FindAsync(id);
                if (personaToUpdate == null) return NotFound();

                if (foto != null)
                {
                    // (Aquí podrías añadir lógica para borrar la foto antigua si existe)
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/personal");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                    }
                    personaToUpdate.FotoUrl = $"/img/personal/{uniqueFileName}";
                }

                personaToUpdate.NombreCompleto = personal.NombreCompleto;
                personaToUpdate.Cargo = personal.Cargo;
                personaToUpdate.Estado = personal.Estado;

                _context.Update(personaToUpdate);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Miembro del personal actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al actualizar al miembro del personal.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Elimina
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var persona = await _context.Personal.FindAsync(id);
            if (persona != null)
            {
                // (Aquí podrías añadir lógica para borrar su foto del servidor)
                _context.Personal.Remove(persona);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Miembro del personal eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}