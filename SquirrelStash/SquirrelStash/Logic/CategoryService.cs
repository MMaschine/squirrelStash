using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Requests;
using SquirrelStash.Resources;

namespace SquirrelStash.Logic
{
    internal class CategoryService(StashContext context, ILogger<CategoryService> logger) : ICategoryService
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
                await MessageHelper.NotifyException(e, "Failed to load categories.", logger);
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
                await MessageHelper.NotifyException(e, $"Failed to create category with title {request.Title}.", logger);
                return Result.Fail(AppText.FailedToCreateCategory);
            }
        }
    }
}
