using Application.Common;
using MediatR;

public class UpdateCategoryCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
