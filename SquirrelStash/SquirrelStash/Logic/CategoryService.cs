using FluentResults;
using Microsoft.EntityFrameworkCore;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Logic
{
    internal class CategoryService(IGenericDataSource<Category> dataSource) : ICategoryService
    {
        public async Task<Result<IReadOnlyList<Category>>> GetCategoriesAsync()
        {
            try
            {
                var data = await dataSource.GetQueryableItems()
                    .Include(x => x.Properties)
                    .Include(x => x.Items)
                        .ThenInclude(x => x.PropertyEntries)
                            .ThenInclude(x => x.Definition).ToListAsync() ?? [];

                return data.AsReadOnly();
            }
            catch (Exception e)
            {
                //TODO: add log
                return Result.Fail("Can't get categories");
            }
        }

        public async Task<Result> CreateCategoryAsync(CreateCategoryRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Properties);

            if (string.IsNullOrEmpty(request.Title))
                throw new ArgumentException(nameof(request.Title));

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

                await dataSource.AddAsync(categoryToAdd);

                return Result.Ok();
            }
            catch (Exception e)
            {
                //TODO: log it
                return Result.Fail("Can't add Category because of exception");
            }
        }
    }
}
