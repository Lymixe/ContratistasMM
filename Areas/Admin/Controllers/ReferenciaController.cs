using ContratistasMM.Areas.Admin.Models;
using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var viewModel = new GestionReferenciaViewModel
            {
                Recursos = await _context.RecursosReferencia.Include(r => r.CategoriaReferencia).OrderBy(r => r.Titulo).ToListAsync(),
                Categorias = await _context.CategoriasReferencia.Include(c => c.Recursos).OrderBy(c => c.Nombre).ToListAsync()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Crear()
        {
            ViewBag.Categorias = new SelectList(await _context.CategoriasReferencia.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre");
            return PartialView("_CrearEditarRecursoModal", new RecursoReferencia());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RecursoReferencia recurso, IFormFile? archivoPdf)
        {
            if (recurso.CategoriaReferenciaId == 0 || string.IsNullOrWhiteSpace(recurso.Titulo) || string.IsNullOrWhiteSpace(recurso.TipoRecurso))
            {
                TempData["ErrorMessage"] = "Hubo un error al crear el recurso. Asegúrate de rellenar todos los campos obligatorios (Título, Categoría, Tipo).";
                return RedirectToAction(nameof(Index));
            }

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

        public async Task<IActionResult> Editar(int id)
        {
            var recurso = await _context.RecursosReferencia.FindAsync(id);
            if (recurso == null) return NotFound();
            ViewBag.Categorias = new SelectList(await _context.CategoriasReferencia.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre", recurso.CategoriaReferenciaId);
            return PartialView("_CrearEditarRecursoModal", recurso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, RecursoReferencia recurso, IFormFile? archivoPdf)
        {
            if (id != recurso.Id) return NotFound();

            if (recurso.CategoriaReferenciaId == 0 || string.IsNullOrWhiteSpace(recurso.Titulo) || string.IsNullOrWhiteSpace(recurso.TipoRecurso))
            {
                TempData["ErrorMessage"] = "Hubo un error al actualizar el recurso. Asegúrate de rellenar todos los campos obligatorios (Título, Categoría, Tipo).";
                return RedirectToAction(nameof(Index));
            }
            
            var recursoToUpdate = await _context.RecursosReferencia.FindAsync(id);
            if (recursoToUpdate == null) return NotFound();

            if (recurso.TipoRecurso == "PDF" && archivoPdf != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documentos/referencia");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(archivoPdf.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await archivoPdf.CopyToAsync(fileStream); }
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

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var recurso = await _context.RecursosReferencia.FindAsync(id);
            if (recurso != null)
            {
                _context.RecursosReferencia.Remove(recurso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Recurso eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(string nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var categoriaExistente = await _context.CategoriasReferencia.FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
                if (categoriaExistente == null)
                {
                    _context.CategoriasReferencia.Add(new CategoriaReferencia { Nombre = nombre });
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Categoría creada exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Esa categoría ya existe.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.CategoriasReferencia.Include(c => c.Recursos).FirstOrDefaultAsync(c => c.Id == id);
            if (categoria != null)
            {
                if (categoria.Recursos.Any())
                {
                    TempData["ErrorMessage"] = "No se puede eliminar la categoría porque tiene recursos asociados.";
                    return RedirectToAction(nameof(Index));
                }
                _context.CategoriasReferencia.Remove(categoria);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}