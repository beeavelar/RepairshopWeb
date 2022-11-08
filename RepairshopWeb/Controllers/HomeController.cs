using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepairshopWeb.Data.Repositories;

namespace RepairshopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMechanicRepository _mechanicRepository;

        public HomeController(ILogger<HomeController> logger, IMechanicRepository mechanicRepository)
        {
            _logger = logger;
            _mechanicRepository = mechanicRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_mechanicRepository.GetAll());
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
