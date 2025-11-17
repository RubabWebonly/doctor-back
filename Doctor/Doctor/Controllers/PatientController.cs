using Doctor.Application.CQRS.Patients.Commands;
using Doctor.Application.CQRS.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreatePatientCommand cmd)
        {
            var id = await _mediator.Send(cmd);

            // 🔹 Yaradılan pasiyentin tam məlumatını gətir
            var patient = await _mediator.Send(new GetPatientByIdQuery { Id = id });

            // 🔹 Frontend-in gözlədiyi formatda cavab
            return Ok(new
            {
                id = id,
                data = patient
            });
        }



        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPatientsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPatientByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdatePatientCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok("Pasiyent məlumatı yeniləndi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePatientCommand { Id = id });
            return Ok("Pasiyent silindi.");
        }


        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] string range = "weekly", [FromQuery] int year = 0)
        {
            if (year == 0) year = DateTime.Now.Year;
            var result = await _mediator.Send(new GetPatientStatsQuery { Range = range, Year = year });
            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var result = await _mediator.Send(new SearchPatientQuery { Query = query });
            return Ok(result);
        }

    }
}
