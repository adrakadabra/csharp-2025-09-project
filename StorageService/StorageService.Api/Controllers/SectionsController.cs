using Microsoft.AspNetCore.Mvc;
using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;

namespace StorageService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly ILogger<SectionsController> _logger;
        private readonly ISectionService _service;
        public SectionsController(ISectionService service, ILogger<SectionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] string code, string? description = null)
        {
            var created = await _service.CreateAsync(code, description);
            return CreatedAtAction(nameof(GetByIdAsync), new { created.Id }, created);
        }

        [ActionName("GetByIdAsync")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [ActionName("GetByCodeAsync")]
        [HttpGet("{code:string}")]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            var p = await _service.GetByCodeAsync(code);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionsAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateSectionDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
