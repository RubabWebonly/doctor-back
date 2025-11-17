using Application.CQRS.AppointmentSlots.Commands;
using Application.CQRS.AppointmentSlots.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentSlotController : ControllerBase
    {
        private readonly ISender _sender;

        public AppointmentSlotController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateAppointmentSlotsCommand command)
        {
            var result = await _sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sender.Send(new GetAllSlotsQuery());
            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] DateTime date)
        {
            var result = await _sender.Send(new GetAvailableSlotsQuery { Date = date });
            return Ok(result);
        }
    }
}
