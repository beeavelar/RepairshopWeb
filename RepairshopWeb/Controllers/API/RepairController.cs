using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;

namespace RepairshopWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {
        private readonly IRepairRepository _repairRepository;

        public RepairController(IRepairRepository repairRepository)
        {
            _repairRepository = repairRepository;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            return Ok(_repairRepository.GetAllWithUsers());
        }
    }
}
