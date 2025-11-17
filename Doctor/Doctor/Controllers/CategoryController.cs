using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCategoryCommand cmd)
    {
        return Ok(await _mediator.Send(cmd));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateCategoryCommand cmd)
    {
        return Ok(await _mediator.Send(cmd));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _mediator.Send(new DeleteCategoryCommand { Id = id }));
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllCategoriesQuery()));
    }
}
