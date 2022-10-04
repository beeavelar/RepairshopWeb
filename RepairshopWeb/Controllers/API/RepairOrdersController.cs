using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;

namespace RepairshopWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IRepairOrderRepository _repairOrderRepository;

        public RepairOrdersController(IRepairOrderRepository repairOrderRepository)
        {
            _repairOrderRepository = repairOrderRepository;
        }

        [HttpGet]
        public IActionResult GetRepairOrdes()
        {
            return Ok(_repairOrderRepository.GetAllWithUsers());
        }
    }
}
