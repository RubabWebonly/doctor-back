using Application.Common;
using MediatR;

public class CreateCategoryCommand : IRequest<Result<int>>
{
    public string Name { get; set; }
}
