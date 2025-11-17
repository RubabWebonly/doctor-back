using Application.CQRS.SystemSettings.Commands;
using Doctor.Application.CQRS.SystemSettings.Commands;
using Doctor.Application.CQRS.SystemSettings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingController : ControllerBase
    {
        private readonly ISender _sender;

        public SystemSettingController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Sistemin slot intervalları və iş saatlarını yeniləyir.
        /// </summary>
        /// <remarks>
        /// Məsələn:
        /// 
        ///     PUT /api/SystemSetting/update
        ///     {
        ///         "slotIntervalMinutes": 40,
        ///         "workStartTime": "09:00:00",
        ///         "workEndTime": "19:00:00"
        ///     }
        /// </remarks>
        /// 

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var result = await _sender.Send(new GetSystemSettingQuery());
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSystemSettingCommand command)
        {
            var result = await _sender.Send(command);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
