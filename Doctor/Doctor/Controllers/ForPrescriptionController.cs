using Doctor.Application.CQRS.ForPrescriptions.Commands;
using Doctor.Application.CQRS.ForPrescriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForPrescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForPrescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateForPrescriptionCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(new { success = true, data = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteForPrescriptionCommand { Id = id });
            return Ok(new { success = ok });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _mediator.Send(new GetAllForPrescriptionsQuery());
            return Ok(new { success = true, data = res });
        }
    }
}
