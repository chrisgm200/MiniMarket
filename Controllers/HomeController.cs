using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniMarketWebApp.Models;

namespace MiniMarketWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Página principal del sistema
        public IActionResult Index()
        {
            ViewData["Title"] = "Inicio - MiniMarket";
            return View();
        }

        // Página de política de privacidad (opcional)
        public IActionResult Privacy()
        {
            ViewData["Title"] = "Política de Privacidad";
            return View();
        }

        // Vista de error global
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult ErrorGeneral()
        {
            return View("ErrorGeneral");
        }

    }
}
