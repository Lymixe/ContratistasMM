using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReferenciaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReferenciaController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var recursos = await _context.RecursosReferencia.Include(r => r.CategoriaReferencia).ToListAsync();
            return View(recursos);
        }

        public async Task<IActionResult> Crear()
        {
            ViewBag.Categorias = new SelectList(await _context.CategoriasReferencia.ToListAsync(), "Id", "Nombre");
            return PartialView("_CrearEditarRecursoModal", new RecursoReferencia());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RecursoReferencia recurso, IFormFile? archivoPdf)
        {
            if (ModelState.IsValid)
            {
                if (recurso.TipoRecurso == "PDF" && archivoPdf != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documentos/referencia");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(archivoPdf.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivoPdf.CopyToAsync(fileStream);
                    }
                    recurso.Url = $"/documentos/referencia/{uniqueFileName}";
                }

                _context.Add(recurso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Recurso creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al crear el recurso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var recurso = await _context.RecursosReferencia.FindAsync(id);
            if (recurso == null) return NotFound();
            ViewBag.Categorias = new SelectList(await _context.CategoriasReferencia.ToListAsync(), "Id", "Nombre", recurso.CategoriaReferenciaId);
            return PartialView("_CrearEditarRecursoModal", recurso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, RecursoReferencia recurso, IFormFile? archivoPdf)
        {
            if (id != recurso.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var recursoToUpdate = await _context.RecursosReferencia.FindAsync(id);
                if (recursoToUpdate == null) return NotFound();

                if (recurso.TipoRecurso == "PDF" && archivoPdf != null)
                {
                    // Lógica para borrar archivo antiguo si existe...
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documentos/referencia");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(archivoPdf.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivoPdf.CopyToAsync(fileStream);
                    }
                    recursoToUpdate.Url = $"/documentos/referencia/{uniqueFileName}";
                }
                else if (recurso.TipoRecurso != "PDF")
                {
                    recursoToUpdate.Url = recurso.Url;
                }

                recursoToUpdate.Titulo = recurso.Titulo;
                recursoToUpdate.Descripcion = recurso.Descripcion;
                recursoToUpdate.TipoRecurso = recurso.TipoRecurso;
                recursoToUpdate.CategoriaReferenciaId = recurso.CategoriaReferenciaId;

                _context.Update(recursoToUpdate);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Recurso actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al actualizar el recurso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var recurso = await _context.RecursosReferencia.FindAsync(id);
            if (recurso != null)
            {
                // Lógica para borrar el archivo físico si es PDF...
                _context.RecursosReferencia.Remove(recurso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Recurso eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}