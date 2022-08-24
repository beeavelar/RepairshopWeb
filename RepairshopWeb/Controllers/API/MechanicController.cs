using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;

namespace RepairshopWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MechanicController : ControllerBase
    {
        private readonly IMechanicRepository _mechanicRepository;
        public MechanicController(IMechanicRepository mechanicRepository)
        {
            _mechanicRepository = mechanicRepository;
        }

        [HttpGet]
        public IActionResult GetMechanics()
        {
            return Ok(_mechanicRepository.GetAllWithUsers());
        }
    }
}
