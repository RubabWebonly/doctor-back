using Doctor.Application.CQRS.PatientPrescriptions.Commands;
using Doctor.Application.CQRS.PatientPrescriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientPrescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientPrescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePatientPrescriptionCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPatientPrescriptionsQuery());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePatientPrescriptionCommand { Id = id });
            return result ? Ok("Silindi") : NotFound("Tapılmadı");
        }
    }
}
