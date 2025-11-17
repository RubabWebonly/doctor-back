using Application.Common;
using MediatR;

public class DeleteCategoryCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
