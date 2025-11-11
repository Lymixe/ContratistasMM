using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ContratistasMM.Models;
using Microsoft.EntityFrameworkCore;
using ContratistasMM.Data;

namespace ContratistasMM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // <-- AÑADIR ESTE CAMPO

        // MODIFICAR EL CONSTRUCTOR para que reciba ApplicationDbContext
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // <-- GUARDAR LA INSTANCIA
        }

        public IActionResult Index()
        {
            return View();
        }

        // ... (otros métodos como Privacy y Error se mantienen igual) ...

        public async Task<IActionResult> Soporte()
        {
            var viewModel = new SoporteViewModel
            {
                // CORREGIR la propiedad de navegación a CategoriaReferencia
                Recursos = await _context.RecursosReferencia.Include(r => r.CategoriaReferencia).OrderBy(r => r.Titulo).ToListAsync(),
                Categorias = await _context.CategoriasReferencia.OrderBy(c => c.Nombre).ToListAsync(),
                FAQs = await _context.PreguntasFrecuentes.OrderBy(f => f.Orden).ToListAsync()
            };
            return View(viewModel);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}