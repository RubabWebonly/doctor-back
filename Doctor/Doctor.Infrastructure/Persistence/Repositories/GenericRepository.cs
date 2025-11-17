using Doctor.Application.Interfaces.Repositories;
using Doctor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // 🔹 Bütün məlumatlar (sadə)
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // 🔹 Overload — Include-lar üçün (relation fetch)
        public virtual async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null)
                query = include(query);
            return await query.ToListAsync();
        }

        // 🔹 ID ilə tapmaq
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // 🔹 Əlavə etmək
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // 🔹 Yeniləmək
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // 🔹 Silmək
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // 🔹 Yadda saxlamaq
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // 🔹 Şərtə uyğun ilk tapılanı gətir
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }


        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }



        public async Task<IEnumerable<Treatment>> GetAllWithDiagnosisAsync()
        {
            if (typeof(T) == typeof(Treatment))
            {
                return (IEnumerable<Treatment>)(object)await _context.Treatments
                    .Include(t => t.Diagnosis)
                    .ToListAsync();
            }

            throw new InvalidOperationException("Bu metod yalnız Treatment üçün istifadə edilə bilər.");
        }

    }
}
