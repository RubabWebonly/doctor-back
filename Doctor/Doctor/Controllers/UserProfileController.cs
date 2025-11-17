using Doctor.Application.CQRS.UserProfiles.Commands;
using Doctor.Application.CQRS.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;

        public UserProfileController(IMediator mediator, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _env = env;
        }

        // ✅ Profil məlumatını yarat / yenilə
        [HttpPut("upsert")]
        public async Task<IActionResult> Upsert([FromForm] UpsertUserProfileCommand cmd)
        {
            try
            {
                // 🔹 Əgər şəkil varsa, yaddaşa yaz
                if (Request.Form.Files.Any())
                {
                    var file = Request.Form.Files[0];
                    var folder = Path.Combine(_env.WebRootPath, "uploads", "profile");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                    cmd.ProfileImageUrl = $"/uploads/profile/{fileName}";
                }

                var result = await _mediator.Send(cmd);
                return Ok(new { success = true, message = "Profil məlumatı yeniləndi", result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ✅ Email ilə profil məlumatını gətir
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var result = await _mediator.Send(new GetUserProfileQuery { Email = email });
                if (result == null)
                    return NotFound(new { success = false, message = "Profil tapılmadı" });

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
