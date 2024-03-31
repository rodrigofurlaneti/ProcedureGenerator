using Microsoft.AspNetCore.Mvc;
using ProcedureGenerator.Web.Models;
using ProcedureGenerator.Web.Services;
using System.Diagnostics;

namespace ProcedureGenerator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Gateway")]
        [HttpPost]
        public IActionResult Gateway()
        {
            var template = string.Empty;

            var modelFormCollection = Request.Form;
            
            if (modelFormCollection != null)
            {
                var model = HomeService.DeparaController(modelFormCollection);

                template = HomeService.Template(model);
            }

            ViewBag.Gateway = template;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}