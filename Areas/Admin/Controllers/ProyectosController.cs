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
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProyectosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProyectosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var proyectos = await _context.Proyectos
                                          .Include(p => p.Cliente)
                                          .OrderByDescending(p => p.Id)
                                          .ToListAsync();
            return View(proyectos);
        }

        public async Task<IActionResult> Crear()
        {
            var viewModel = new ProyectoViewModel
            {
                Clientes = await GetClientesSelectList()
            };
            return PartialView("_CrearProyectoModal", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProyectoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var proyecto = new Proyecto
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Ubicacion = model.Ubicacion,
                    AnioEjecucion = model.AnioEjecucion,
                    TipoObra = model.TipoObra,
                    Estado = "En Progreso",
                    Progreso = 0,
                    EsPublico = model.EsPublico,
                    ClienteId = model.SelectedClienteId
                };
                _context.Add(proyecto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Proyecto creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al crear. Revisa los datos.";
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();
            
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null) return NotFound();

            var viewModel = new ProyectoViewModel
            {
                Id = proyecto.Id,
                Nombre = proyecto.Nombre,
                Descripcion = proyecto.Descripcion,
                Ubicacion = proyecto.Ubicacion,
                AnioEjecucion = proyecto.AnioEjecucion,
                TipoObra = proyecto.TipoObra,
                Estado = proyecto.Estado,
                Progreso = proyecto.Progreso,
                EsPublico = proyecto.EsPublico,
                SelectedClienteId = proyecto.ClienteId,
                Clientes = await GetClientesSelectList()
            };
            return PartialView("_EditarProyectoModal", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ProyectoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var proyecto = await _context.Proyectos.FindAsync(id);
                if (proyecto == null) return NotFound();

                proyecto.Nombre = model.Nombre;
                proyecto.Descripcion = model.Descripcion;
                proyecto.Ubicacion = model.Ubicacion;
                proyecto.AnioEjecucion = model.AnioEjecucion;
                proyecto.TipoObra = model.TipoObra;
                proyecto.Estado = model.Estado;
                proyecto.Progreso = model.Progreso;
                proyecto.EsPublico = model.EsPublico;
                proyecto.ClienteId = model.SelectedClienteId;
                
                _context.Update(proyecto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Proyecto actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hubo un error al actualizar. Revisa los datos.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                _context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Proyecto eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetClientesSelectList()
        {
            var clientes = await _userManager.GetUsersInRoleAsync("Cliente");
            return clientes.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = $"{c.Nombre} {c.Apellido}"
            }).ToList();
        }

        [HttpGet]
        public IActionResult CargarDocumento(int proyectoId)
        {
            var viewModel = new CargarDocumentoViewModel { ProyectoId = proyectoId };
            return PartialView("_CargarDocumentoModal", viewModel);
        }

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
                    
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Archivo.FileName);
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
                    UrlArchivo = uniqueFileName, 
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