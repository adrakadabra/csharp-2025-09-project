using Microsoft.AspNetCore.Mvc;
using StorageService.Api.Application.Interfaces;

namespace StorageService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService _service;

        public ManufacturersController(IManufacturerService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var manufacturer = await _service.GetAsync(id);
            if (manufacturer == null) return NotFound();

            return Ok(manufacturer);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
