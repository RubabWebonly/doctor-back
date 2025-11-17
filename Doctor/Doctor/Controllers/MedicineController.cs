using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MedicineController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicineController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var data = await _mediator.Send(new GetAllMedicinesQuery());
        return Ok(new { success = true, data });
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateMedicineCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return Ok(new { success = true, id });
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateMedicineCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return Ok(new { success = result });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteMedicineCommand { Id = id });
        return Ok(new { success = result });
    }
}
