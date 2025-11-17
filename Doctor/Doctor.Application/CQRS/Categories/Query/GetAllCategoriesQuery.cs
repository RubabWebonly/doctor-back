using Domain.Entities;
using MediatR;

public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
{
}
