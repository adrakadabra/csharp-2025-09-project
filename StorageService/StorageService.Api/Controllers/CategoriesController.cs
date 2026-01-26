using Microsoft.AspNetCore.Mvc;
using StorageService.Api.Application.Interfaces;

namespace StorageService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _service.GetAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
