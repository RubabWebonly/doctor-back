using Doctor.Application.CQRS.Clinics.Commands;
using Doctor.Application.CQRS.Clinics.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClinicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateClinicCommand cmd)
            => Ok(await _mediator.Send(cmd));

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateClinicCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok("Klinika yeniləndi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteClinicCommand { Id = id });
            return Ok("Klinika silindi.");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetAllClinicsQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetClinicByIdQuery { Id = id }));
    }
}
