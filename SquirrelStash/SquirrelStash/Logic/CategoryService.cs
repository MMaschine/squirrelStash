using FluentResults;
using Microsoft.EntityFrameworkCore;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;
using SquirrelStash.Resources;

namespace SquirrelStash.Logic
{
    internal class CategoryService(StashContext context) : ICategoryService
    {
        private readonly DbSet<Category> _categoriesSet = context.Set<Category>();

        public async Task<Result<IReadOnlyList<Category>>> GetCategoriesAsync()
        {
            try
            {
                var data = await _categoriesSet.AsQueryable()
                    .Include(x => x.Properties)
                    .Include(x => x.Items)
                        .ThenInclude(x => x.PropertyEntries)
                            .ThenInclude(x => x.Definition)
                    .AsNoTracking().ToListAsync() ?? [];

                return data.AsReadOnly();
            }
            catch (Exception e)
            {
                //TODO: add log
                return Result.Fail(AppText.CannotGetCategories);
            }
        }

        public async Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Properties);

            if (string.IsNullOrEmpty(request.Title))
            {
                throw new ArgumentException(nameof(request.Title));
            }

            try
            {
                var categoryToAdd = new Category()
                {
                    Title = request.Title
                };

                categoryToAdd.Properties.AddRange(request.Properties.Select(x => new PropertyDefinition()
                {
                    TypeCode = (int)x.Type,
                    Name = x.Name,
                    AllowedValues = x.AllowedValues
                }));

                await _categoriesSet.AddAsync(categoryToAdd);
                await context.SaveChangesAsync();

                return Result.Ok(categoryToAdd);
            }
            catch (Exception e)
            {
                //TODO: log it
                return Result.Fail(AppText.FailedToCreateCategory);
            }
        }
    }
}
