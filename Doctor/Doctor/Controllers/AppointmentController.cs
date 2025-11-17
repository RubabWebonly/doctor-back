using Doctor.Application.CQRS.Appointments.Commands;
using Doctor.Application.CQRS.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand cmd)
            => Ok(await _mediator.Send(cmd));

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateAppointmentCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok("Randevu yeniləndi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAppointmentCommand { Id = id });
            return Ok("Randevu silindi.");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetAllAppointmentsQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetAppointmentByIdQuery { Id = id }));

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] string range = "weekly", [FromQuery] int year = 0)
        {
            if (year == 0) year = DateTime.Now.Year;
            var result = await _mediator.Send(new GetAppointmentStatsQuery { Range = range, Year = year });
            return Ok(result);
        }

    }
}
