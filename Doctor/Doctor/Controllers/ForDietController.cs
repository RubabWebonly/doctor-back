using Doctor.Application.CQRS.ForDiets.Commands;
using Doctor.Application.CQRS.ForDiets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForDietController : ControllerBase
    {
        private readonly ISender _sender;

        public ForDietController(ISender sender)
        {
            _sender = sender;
        }

        // ===========================
        //  📌 GET ALL
        // ===========================
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sender.Send(new GetAllForDietsQuery());
            return Ok(new { success = true, data = result });
        }

        // ===========================
        //  📌 CREATE
        // ===========================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateForDietCommand req)
        {
            var result = await _sender.Send(req);
            return Ok(new { success = true, data = result, message = "Diet PDF created" });
        }

        // ===========================
        //  📌 DELETE (soft delete)
        // ===========================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sender.Send(new DeleteForDietCommand { Id = id });

            if (!result)
                return NotFound(new { success = false, message = "Not found" });

            return Ok(new { success = true, message = "Deleted successfully" });
        }
    }
}
