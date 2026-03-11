using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.DataAccess.DataSource
{
    public class GenericDataSource<T>(StashContext context) where T : BaseEntity //: IGenericDataSource<T> 
    {
        private readonly DbSet<T> _set = context.Set<T>();

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _set.AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _set.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetItemsAsync(Expression<Func<T, bool>> filter)
        {
            var entities = await _set.Where(filter)
                .AsNoTracking()
                .ToListAsync();

            return entities.AsReadOnly();
        }

        /// <inheritdoc />
        public IQueryable<T> GetQueryableItems()
        {
            return _set.AsNoTracking();
        }

        /// <inheritdoc />
        public async Task<T> AddAsync(T item)
        {
            await _set.AddAsync(item);
            await context.SaveChangesAsync();

            return item;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(T item)
        {
            _set.Update(item);
            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(T item)
        {
            _set.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
