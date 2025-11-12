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
public class ServiciosAdminController : Controller
{
    private readonly ApplicationDbContext _context;
    public ServiciosAdminController(ApplicationDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var servicios = await _context.Servicios.ToListAsync();
        return View(servicios);
    }

    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();
        var servicio = await _context.Servicios.FindAsync(id);
        if (servicio == null) return NotFound();
        return View(servicio);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Descripcion,Beneficios,Caracteristicas")] Servicio servicio)
    {
        if (id != servicio.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(servicio);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Servicio actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(servicio);
    }
}