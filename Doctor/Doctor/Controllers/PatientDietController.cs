using Doctor.Application.CQRS.PatientDiets.Commands;
using Doctor.Application.CQRS.PatientDiets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientDietController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientDietController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePatientDietCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPatientDietsQuery());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePatientDietCommand { Id = id });
            return result ? Ok("Silindi") : NotFound("Tapılmadı");
        }
    }
}
