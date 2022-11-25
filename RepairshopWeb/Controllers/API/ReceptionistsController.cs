using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;

namespace RepairshopWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistsController : ControllerBase
    {
        private readonly IReceptionistRepository _receptionistRepository;

        public ReceptionistsController(IReceptionistRepository receptionistRepository)
        {
            _receptionistRepository = receptionistRepository;
        }

        [HttpGet]
        public IActionResult GetReceptionists()
        {
            return Ok(_receptionistRepository.GetAllWithUsers());
        }
    }
}
