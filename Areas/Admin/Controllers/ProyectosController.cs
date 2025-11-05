using ContratistasMM.Areas.Admin.Models;
using ContratistasMM.Data;
using ContratistasMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        var proyectos = await _context.Proyectos.Include(p => p.Cliente).OrderByDescending(p => p.Id).ToListAsync();
        return View(proyectos);
    }

    public async Task<IActionResult> Crear()
    {
        var viewModel = new ProyectoViewModel { Clientes = await GetClientesSelectList() };
        return PartialView("_CrearProyectoModal", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProyectoViewModel model)
    {
        if (ModelState.IsValid)
        {
            string imageUrl = null;
            if (model.ImagenPrincipal != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/proyectos");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImagenPrincipal.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await model.ImagenPrincipal.CopyToAsync(fileStream); }
                imageUrl = $"/img/proyectos/{uniqueFileName}";
            }

            var proyecto = new Proyecto
            {
                Nombre = model.Nombre, Descripcion = model.Descripcion, Ubicacion = model.Ubicacion,
                AnioEjecucion = model.AnioEjecucion, TipoObra = model.TipoObra, Estado = "En Progreso",
                Progreso = 0, EsPublico = model.EsPublico, ClienteId = model.SelectedClienteId,
                FechaInicio = model.FechaInicio.HasValue ? DateTime.SpecifyKind(model.FechaInicio.Value, DateTimeKind.Utc) : null,
                ImagenUrl = imageUrl
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
            Id = proyecto.Id, Nombre = proyecto.Nombre, Descripcion = proyecto.Descripcion, Ubicacion = proyecto.Ubicacion,
            AnioEjecucion = proyecto.AnioEjecucion, TipoObra = proyecto.TipoObra, FechaInicio = proyecto.FechaInicio,
            Estado = proyecto.Estado, Progreso = proyecto.Progreso, EsPublico = proyecto.EsPublico,
            SelectedClienteId = proyecto.ClienteId, Clientes = await GetClientesSelectList(), ExistingImageUrl = proyecto.ImagenUrl
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

            if (model.ImagenPrincipal != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/proyectos");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImagenPrincipal.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await model.ImagenPrincipal.CopyToAsync(fileStream); }
                proyecto.ImagenUrl = $"/img/proyectos/{uniqueFileName}";
            }

            proyecto.Nombre = model.Nombre; proyecto.Descripcion = model.Descripcion; proyecto.Ubicacion = model.Ubicacion;
            proyecto.AnioEjecucion = model.AnioEjecucion; proyecto.TipoObra = model.TipoObra;
            proyecto.FechaInicio = model.FechaInicio.HasValue ? DateTime.SpecifyKind(model.FechaInicio.Value, DateTimeKind.Utc) : null;
            proyecto.Estado = model.Estado; proyecto.Progreso = model.Progreso; proyecto.EsPublico = model.EsPublico;
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
        return clientes.Select(c => new SelectListItem { Value = c.Id, Text = $"{c.Nombre} {c.Apellido}" }).ToList();
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
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await model.Archivo.CopyToAsync(fileStream); }
            }
            var documento = new Documento
            {
                Nombre = model.Nombre,
                Clasificacion = model.Clasificacion,
                UrlArchivo = uniqueFileName,
                ProyectoId = model.ProyectoId,
                EsVisibleParaCliente = model.EsVisibleParaCliente
            };
            _context.Add(documento);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Documento cargado y vinculado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = "Hubo un error al cargar el documento.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detalle(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var proyecto = await _context.Proyectos
            .Include(p => p.Hitos)        // Carga los hitos relacionados
                .ThenInclude(h => h.ArchivosHito) // Carga los archivos de cada hito
            .Include(p => p.Documentos)   // Carga los documentos relacionados
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proyecto == null)
        {
            return NotFound();
        }

        var viewModel = new DetalleProyectoViewModel
        {
            Proyecto = proyecto
        };

        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult CrearHito(int proyectoId)
    {
        var viewModel = new HitoViewModel
        {
            ProyectoId = proyectoId
        };
        return PartialView("_CrearHitoModal", viewModel);
    }

    // POST: /Admin/Proyectos/CrearHito
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearHito(HitoViewModel model)
    {
        if (ModelState.IsValid)
        {
            var hito = new Hito
            {
                ProyectoId = model.ProyectoId,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                FechaEstimada = model.FechaEstimada.HasValue
                    ? DateTime.SpecifyKind(model.FechaEstimada.Value, DateTimeKind.Utc)
                    : null,
                Estado = model.Estado
            };

            // Procesar y guardar múltiples archivos
            if (model.Archivos != null && model.Archivos.Any())
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/hitos");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                foreach (var archivo in model.Archivos)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(archivo.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivo.CopyToAsync(fileStream);
                    }

                    var archivoHito = new ArchivoHito
                    {
                        UrlArchivo = $"/img/hitos/{uniqueFileName}",
                        Tipo = archivo.ContentType.StartsWith("image") ? "Imagen" : "Video"
                    };
                    hito.ArchivosHito.Add(archivoHito);
                }
            }

            _context.Hitos.Add(hito);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Hito agregado exitosamente.";
        }
        else
        {
            TempData["ErrorMessage"] = "Hubo un error al crear el hito. Revisa los datos.";
        }

        return RedirectToAction("Detalle", new { id = model.ProyectoId });
    }
    
    [HttpPost]
    public async Task<IActionResult> CambiarVisibilidadDocumento(int id, bool esVisible)
    {
        var documento = await _context.Documentos.FindAsync(id);
        if (documento == null)
        {
            return NotFound();
        }
        documento.EsVisibleParaCliente = esVisible;
        _context.Update(documento);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Visibilidad actualizada." }); // Devuelve una respuesta exitosa
    }

}