using ContratistasMM.Areas.Admin.Models;
using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProyectosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProyectosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Muestra la lista de todos los proyectos
        public async Task<IActionResult> Index()
        {
            var proyectos = await _context.Proyectos
                                          .Include(p => p.Cliente) // Incluimos el cliente para mostrar su nombre
                                          .OrderByDescending(p => p.Id)
                                          .ToListAsync();
            return View(proyectos);
        }

        // Muestra el modal para cargar un documento
        [HttpGet]
        public IActionResult CargarDocumento(int proyectoId)
        {
            var viewModel = new CargarDocumentoViewModel
            {
                ProyectoId = proyectoId
            };
            return PartialView("_CargarDocumentoModal", viewModel);
        }

        // Procesa el archivo subido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarDocumento(CargarDocumentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Archivo != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documentos");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Archivo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Archivo.CopyToAsync(fileStream);
                    }
                }

                var documento = new Documento
                {
                    Nombre = model.Nombre,
                    Clasificacion = model.Clasificacion,
                    UrlArchivo = uniqueFileName, // Guardamos solo el nombre del archivo
                    ProyectoId = model.ProyectoId
                };

                _context.Add(documento);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Documento cargado y vinculado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Hubo un error al cargar el documento.";
            return RedirectToAction(nameof(Index));
        }
    }
}