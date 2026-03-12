using Microsoft.AspNetCore.Mvc;
using StorageService.Api.Application.Interfaces;

namespace StorageService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationItemsController : ControllerBase
    {
        private readonly IReservationItemsService _service;

        public ReservationItemsController(IReservationItemsService service)
        {
            _service = service;
        }

        [HttpPut("pickForAssembly")]
        public async Task<IActionResult> AssemblyAsync(Guid orderNumber, string article)
        {
            await _service.ChangeReservationItemStatusAsync(orderNumber, article, Common.Enums.ReservationItemStatus.InAssembly);
            return Ok();
        }

        [HttpPut("cancelFromAssembly")]
        public async Task<IActionResult> CancelAsync(Guid orderNumber, string article)
        {
            await _service.ChangeReservationItemStatusAsync(orderNumber, article, Common.Enums.ReservationItemStatus.Canceled);
            return Ok();
        }

        [HttpPut("returnToReserve")]
        public async Task<IActionResult> ReserveAsync(Guid orderNumber, string article)
        {
            await _service.ChangeReservationItemStatusAsync(orderNumber, article, Common.Enums.ReservationItemStatus.Reserved);
            return Ok();
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetReservationItemsForOrder(Guid orderNumber)
        {
            return Ok(await _service.GetReservationItemsForOrder(orderNumber));
        }
    }
}
