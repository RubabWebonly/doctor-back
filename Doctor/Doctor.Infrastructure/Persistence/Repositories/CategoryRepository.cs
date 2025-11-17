using Domain.Entities;
using Doctor.Application.Interfaces.Repositories;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
