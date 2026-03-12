using Microsoft.AspNetCore.Mvc;
using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;

namespace StorageService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve(ReserveProductsDto dto)
        {
            return Ok(await _service.ReserveProductsAsync(dto));
        }

        [HttpPut("cancel/{orderNumber}")]
        public async Task<IActionResult> Cancel(Guid orderNumber)
        {
            await _service.CancelReservationAsync(orderNumber);
            return Ok();
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetReservationByOrderAsync(Guid orderNumber)
        {
            return Ok(await _service.GetReservationAsync(orderNumber));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllReservationsAsync()
        {
            return Ok(await _service.GetAllReservationsAsync());
        }
    }
}
